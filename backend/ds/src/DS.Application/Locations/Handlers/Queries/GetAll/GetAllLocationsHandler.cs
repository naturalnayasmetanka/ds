using CSharpFunctionalExtensions;
using DS.Application.Abstractions.Database;
using DS.Application.Abstractions.Handlers;
using DS.Application.Locations.Handlers.Queries.Get;
using DS.Contracts.Locations;
using DS.Contracts.Locations.Get;
using DS.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DS.Application.Locations.Handlers.Queries.GetAll
{
    public class GetAllLocationsHandler : IQueryHandler<List<GetLocationsResponse>, GetAllLocationsQuery>
    {
        private readonly IReadDbContext _readDbContext;
        private readonly ILogger<GetLocationHandler> _logger;

        public GetAllLocationsHandler(
            IReadDbContext readDbContext,
            ILogger<GetLocationHandler> logger)
        {
            _readDbContext = readDbContext;
            _logger = logger;
        }

        public async Task<Result<List<GetLocationsResponse>, Errors>> Handle(
            GetAllLocationsQuery query,
            CancellationToken cancellationToken = default)
        {
            var locations = _readDbContext.LocationsRead;

            var locationsResponse = await locations.Select(x => new GetLocationsResponse()
            {
                Id = x.Id,
                Name = x.Name.Value,
                Timezone = x.Timezone.Value,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,

                Address = new AddressDto(
                    x.Address.Country,
                    x.Address.Region,
                    x.Address.SettlementName,
                    x.Address.SettlementType,
                    x.Address.Street,
                    x.Address.BuildingNumber,
                    x.Address.BuildingBlock,
                    x.Address.Entrance,
                    x.Address.Floor,
                    x.Address.PremiseNumber,
                    x.Address.PremiseType,
                    x.Address.PostCode,
                    x.Address.FullAddress,
                    x.Address.Comment)
            }).ToListAsync(cancellationToken);

            return Result.Success<List<GetLocationsResponse>, Errors>(locationsResponse);
        }
    }
}
