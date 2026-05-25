using DS.Contracts.Locations;

namespace DS.Contracts.Locations.Update;

public record UpdateLocationRequest(
    string Name,
    string TimeZone,
    AddressDto Adress);