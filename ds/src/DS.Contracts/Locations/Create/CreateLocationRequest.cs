namespace DS.Contracts.Location.Create;

public record CreateLocationRequest(string Name, string TimeZone, AddressDto Adress);
