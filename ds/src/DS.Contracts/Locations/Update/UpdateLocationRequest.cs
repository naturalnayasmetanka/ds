using DS.Contracts.Locations;

namespace DS.Contracts.Locations.Update;

public record UpdateLocationRequest(string Name, AddressDto Adress);