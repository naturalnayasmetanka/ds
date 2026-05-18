using Dapper;
using DS.Application.Locations.Repositories;
using DS.Domain.Models.Locations;
using DS.Infrastructure.Database.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DS.Infrastructure.Database.Emplementations.Repository;

public class NpgSqlLocationsRepository : ILocationsRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgSqlLocationsRepository> _logger;

    public NpgSqlLocationsRepository(IDbConnectionFactory connectionFactory, ILogger<NpgSqlLocationsRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task AddAsync(Location newLocation, CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        const string locationInsert =
                                    """
                                    INSERT INTO locations 
                                        (id, name, timezone, is_active, created_at, updated_at, address)
                                    VALUES 
                                        (@Id, @Name, @Timezone, @Is_active, @Created_at, @Updated_at, @Address::jsonb)
                                    """;
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var locationInsertParams = new
        {
            Id = newLocation.Id,
            Name = newLocation.Name.Value,
            Timezone = newLocation.Timezone.Value,
            Is_active = newLocation.IsActive,
            Created_at = newLocation.CreatedAt,
            Updated_at = newLocation.UpdatedAt,
            Address = System.Text.Json.JsonSerializer.Serialize(newLocation.Address, options)
        };

        try
        {
            await connection.ExecuteAsync(locationInsert, locationInsertParams);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }


    public async Task<bool> ExistsByNameAsync(Name name, CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        const string isExistsByName = """
                                        SELECT EXISTS (
                                            SELECT 1 
                                            FROM locations 
                                            WHERE name = @Name)
                                        """;

        try
        {
            var exists = await connection.ExecuteScalarAsync<bool>(isExistsByName, new { Name = name.Value });

            return exists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }

        return true;
    }

    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}
