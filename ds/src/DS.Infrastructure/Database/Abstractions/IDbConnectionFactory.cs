using System.Data;

namespace DS.Infrastructure.Database.Abstractions;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default);
}
