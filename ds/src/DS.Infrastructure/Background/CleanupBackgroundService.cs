using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using DS.Infrastructure.Background;
using DS.Application.Abstractions.Database;
using System.Data;
using Npgsql;
using System.Threading.Channels;

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

    public CleanupBackgroundService(
        ILogger<CleanupBackgroundService> logger,
        IDbConnectionFactory connectionFactory,
        IOptions<CleanupOptions> options)
    {
        _logger = logger;
        _connectionFactory = connectionFactory;
        _options = options.Value;
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

        // delete departments and locations soft-deleted (is_active = false) older than cutoff
        await DeleteInBatches("departments", "id", "is_active", "updated_at", cutoff, _options.BatchSize, cancellationToken);
        await DeleteInBatches("locations", "id", "is_active", "updated_at", cutoff, _options.BatchSize, cancellationToken);
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

                var sql = $@"DELETE FROM {tableName} t WHERE t.{idColumn} IN (
    SELECT id FROM {tableName} WHERE {activeColumn} = FALSE AND {timeColumn} < @cutoff LIMIT @batchSize
)";

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
