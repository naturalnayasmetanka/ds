using DS.Application.Abstractions.Database;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DS.Infrastructure.Database.Emplementations;

public class DbConnectionFactory(DsDbContext dbContext) : IDbConnectionFactory
{
    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connection = dbContext.Database.GetDbConnection();

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync(cancellationToken);

        return connection;
    }
}
