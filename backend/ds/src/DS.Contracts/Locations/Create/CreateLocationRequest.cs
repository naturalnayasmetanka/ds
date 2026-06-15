using DS.Contracts.Locations;

namespace DS.Contracts.Locations.Create;

public record CreateLocationRequest(string Name, string TimeZone, AddressDto Adress);
