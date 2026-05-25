using DS.Infrastructure.Database.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Data;

namespace DS.Infrastructure.Database.Emplementations;

public class NpgSqlConnectionFactory : IDbConnectionFactory, IDisposable, IAsyncDisposable
{
    private readonly NpgsqlDataSource _dataSource;
    public NpgSqlConnectionFactory(IConfiguration configuration)
    {
        var dataSpurceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("DsDbConnection"));

        dataSpurceBuilder.UseLoggerFactory(CreateLoggerFactory());

        _dataSource = dataSpurceBuilder.Build();
    }

    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken)
    {
        return await _dataSource.OpenConnectionAsync(cancellationToken);
    }

    private ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => { builder.AddConsole(); });
    }

    public void Dispose()
    {
        _dataSource.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _dataSource.DisposeAsync();
    }
}
