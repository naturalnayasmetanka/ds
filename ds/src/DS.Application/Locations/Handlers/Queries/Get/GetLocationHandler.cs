using CSharpFunctionalExtensions;
using DS.Application.Abstractions.Database;
using DS.Application.Abstractions.Handlers;
using DS.Contracts.Locations;
using DS.Contracts.Locations.GetById;
using DS.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DS.Application.Locations.Handlers.Queries.Get;

public class GetLocationHandler : IQueryHandler<GetLocationResponse?, GetLocationQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<GetLocationHandler> _logger;
    public GetLocationHandler(
        IReadDbContext readDbContext,
        ILogger<GetLocationHandler> logger)
    {
        _readDbContext = readDbContext;
        _logger = logger;
    }

    public async Task<Result<GetLocationResponse?, Errors>> Handle(
        GetLocationQuery query,
        CancellationToken cancellationToken = default)
    {
        var location = await _readDbContext.LocationsRead.FirstOrDefaultAsync(x => x.Id == query.request.Id);

        if (location is null)
            return Result.Success<GetLocationResponse?, Errors>(null);

        var locationResponse = new GetLocationResponse()
        {
            Id = location.Id,
            Name = location.Name.Value,
            Timezone = location.Timezone.Value,
            IsActive = location.IsActive,
            CreatedAt = location.CreatedAt,
            UpdatedAt = location.UpdatedAt,

            Address = new AddressDto(
                location.Address.Country,
                location.Address.Region,
                location.Address.SettlementName,
                location.Address.SettlementType,
                location.Address.Street,
                location.Address.BuildingNumber,
                location.Address.BuildingBlock,
                location.Address.Entrance,
                location.Address.Floor,
                location.Address.PremiseNumber,
                location.Address.PremiseType,
                location.Address.PostCode,
                location.Address.FullAddress,
                location.Address.Comment)
        };

        return Result.Success<GetLocationResponse?, Errors>(locationResponse);
    }
}
