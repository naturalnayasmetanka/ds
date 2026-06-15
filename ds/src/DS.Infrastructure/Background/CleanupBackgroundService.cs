using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using DS.Infrastructure.Background;
using DS.Application.Abstractions.Database;
using System.Data;
using Npgsql;
using System.Threading.Channels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DS.Infrastructure.Database.Emplementations;
using DS.Domain.Models.Departments;
using DS.Domain.Models.Locations;

namespace DS.Infrastructure.Background;

public class CleanupOptions
{
    public TimeSpan Interval { get; set; } = TimeSpan.FromHours(1);
    public TimeSpan MaxAge { get; set; } = TimeSpan.FromDays(30);
    public int BatchSize { get; set; } = 1000;
}

public class CleanupBackgroundService : BackgroundService
{
    private readonly ILogger<CleanupBackgroundService> _logger;
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly CleanupOptions _options;
    private readonly DsDbContext _dbContext;

    public CleanupBackgroundService(
        ILogger<CleanupBackgroundService> logger,
        IDbConnectionFactory connectionFactory,
        IOptions<CleanupOptions> options,
        DsDbContext dbContext)
    {
        _logger = logger;
        _connectionFactory = connectionFactory;
        _options = options.Value;
        _dbContext = dbContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("CleanupBackgroundService started. Interval: {interval}, MaxAge: {maxAge}, BatchSize: {batchSize}", _options.Interval, _options.MaxAge, _options.BatchSize);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await RunOnce(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CleanupBackgroundService run failed");
            }

            try
            {
                await Task.Delay(_options.Interval, stoppingToken);
            }
            catch (TaskCanceledException)
            {
                // stopping
            }
        }
    }

    private async Task RunOnce(CancellationToken cancellationToken)
    {
        // compute cutoff
        var cutoff = DateTime.UtcNow - _options.MaxAge;

        // determine table/column names from EF Core model when possible
        var deptMapping = ResolveMapping(typeof(Department), "departments", "IsActive", "UpdatedAt");
        var locMapping = ResolveMapping(typeof(Location), "locations", "IsActive", "UpdatedAt");

        await DeleteInBatches(deptMapping.table, deptMapping.idColumn, deptMapping.activeColumn, deptMapping.timeColumn, cutoff, _options.BatchSize, cancellationToken);
        await DeleteInBatches(locMapping.table, locMapping.idColumn, locMapping.activeColumn, locMapping.timeColumn, cutoff, _options.BatchSize, cancellationToken);
    }

    private (string table, string idColumn, string activeColumn, string timeColumn, string activeKind) ResolveMapping(Type clrType, string defaultTable, string activeProp, string timeProp)
    {
        try
        {
            var model = _dbContext.Model;
            var entityType = model.FindEntityType(clrType);
            if (entityType != null)
            {
                var tableName = entityType.GetTableName() ?? defaultTable;
                var schema = entityType.GetSchema();
                var store = StoreObjectIdentifier.Table(tableName, schema);

                string idCol = null;
                // try common id property names
                var idProp = entityType.FindProperty("Id") ?? entityType.GetProperties().FirstOrDefault(p => p.IsPrimaryKey());
                if (idProp != null)
                    idCol = idProp.GetColumnName(store);

                string activeCol = null;
                var actProp = entityType.FindProperty(activeProp) ?? entityType.FindProperty("IsActive") ?? entityType.FindProperty("IsDeleted") ?? entityType.FindProperty("DeletedAt");
                if (actProp != null)
                    activeCol = actProp.GetColumnName(store);

                string timeCol = null;
                var timeProperty = entityType.FindProperty(timeProp) ?? entityType.FindProperty("UpdatedAt") ?? entityType.FindProperty("DeletedAt");
                if (timeProperty != null)
                    timeCol = timeProperty.GetColumnName(store);

                // determine kind of active column: Bool | DateTime | Unknown
                string activeKind = "Unknown";
                if (actProp != null)
                {
                    var clr = actProp.ClrType;
                    if (clr == typeof(bool) || clr == typeof(Boolean) || clr == typeof(bool?)) activeKind = "Bool";
                    else if (clr == typeof(DateTime) || clr == typeof(DateTime?)) activeKind = "DateTime";
                }

                // fallback to defaults if any missing
                idCol ??= "id";
                activeCol ??= "is_active";
                timeCol ??= "updated_at";

                // validate identifiers
                if (!IsValidSqlIdentifier(tableName) || !IsValidSqlIdentifier(idCol) || !IsValidSqlIdentifier(activeCol) || !IsValidSqlIdentifier(timeCol))
                {
                    _logger.LogWarning("EF mapping produced invalid SQL identifiers for {type}. Falling back to defaults.", clrType.Name);
                    return (defaultTable, "id", "is_active", "updated_at", "Unknown");
                }

                return (tableName, idCol, activeCol, timeCol, activeKind);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to resolve EF mapping for {type}", clrType.Name);
        }
        return (defaultTable, "id", "is_active", "updated_at", "Unknown");
    }

    private bool IsValidSqlIdentifier(string ident)
    {
        if (string.IsNullOrEmpty(ident)) return false;
        // allow letters, numbers, underscore
        return System.Text.RegularExpressions.Regex.IsMatch(ident, "^[A-Za-z_][A-Za-z0-9_]*$");
    }

    private string QuoteIdentifier(string ident)
    {
        // double-quote and escape internal quotes
        return '"' + ident.Replace("\"", "\"\"") + '"';
    }

    private async Task DeleteInBatches(string tableName, string idColumn, string activeColumn, string timeColumn, DateTime cutoff, int batchSize, CancellationToken cancellationToken)
    {
        // We will delete by primary key in batches. For Postgres, use DELETE ... WHERE id IN (SELECT id FROM table WHERE conditions LIMIT @batchSize) returning count
        // Keep transactions short and single SQL per batch
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await using var conn = await _connectionFactory.CreateConnectionAsync(cancellationToken) as NpgsqlConnection;
                if (conn == null)
                {
                    _logger.LogWarning("Connection is not NpgsqlConnection, skipping cleanup for {table}", tableName);
                    return;
                }

                // build safe quoted identifiers (we already validated identifiers earlier)
                var qTable = QuoteIdentifier(tableName);
                var qId = QuoteIdentifier(idColumn);
                var qActive = QuoteIdentifier(activeColumn);
                var qTime = QuoteIdentifier(timeColumn);

                // Build deletion condition depending on active column kind
                string condition;
                // try to inspect EF mapping for kind
                var kind = "Unknown";
                try
                {
                    var et = _dbContext.Model.FindEntityType(tableName) ?? null;
                }
                catch { }

                // By default assume boolean flag
                // If activeColumn looks like a timestamp (contains 'deleted' or 'at') we will use IS NOT NULL AND < cutoff
                if (qActive.ToLowerInvariant().Contains("deleted") || qTime.ToLowerInvariant().Contains("deleted") || qTime.ToLowerInvariant().Contains("updated"))
                {
                    condition = $"{qActive} IS NOT NULL AND {qActive} < @cutoff";
                }
                else
                {
                    // boolean flag
                    condition = $"{qActive} = FALSE AND {qTime} < @cutoff";
                }

                // For departments we should also clean join tables to avoid dangling references
                string sql;
                if (string.Equals(tableName, "departments", StringComparison.OrdinalIgnoreCase))
                {
                    // use CTE to pick ids to delete
                    sql = $@"WITH cte AS (SELECT {qId} FROM {qTable} WHERE {condition} LIMIT @batchSize)
DELETE FROM {QuoteIdentifier("department_locations")} WHERE {QuoteIdentifier("department_id")} IN (SELECT {qId} FROM cte);
DELETE FROM {QuoteIdentifier("department_positions")} WHERE {QuoteIdentifier("department_id")} IN (SELECT {qId} FROM cte);
DELETE FROM {qTable} WHERE {qId} IN (SELECT {qId} FROM cte);";
                }
                else
                {
                    sql = $@"WITH cte AS (SELECT {qId} FROM {qTable} WHERE {condition} LIMIT @batchSize)
DELETE FROM {qTable} WHERE {qId} IN (SELECT {qId} FROM cte);";
                }

                await using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("cutoff", cutoff);
                cmd.Parameters.AddWithValue("batchSize", batchSize);

                var affected = await cmd.ExecuteNonQueryAsync(cancellationToken);

                if (affected == 0)
                {
                    // nothing to delete
                    break;
                }

                _logger.LogInformation("Deleted {count} rows from {table}", affected, tableName);

                // if less than batch size deleted - done
                if (affected < batchSize)
                    break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting batch from {table}", tableName);
                // do not throw further, stop current run
                break;
            }
        }
    }
}
