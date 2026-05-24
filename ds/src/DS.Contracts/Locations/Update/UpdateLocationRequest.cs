namespace DS.Contracts.Location.Update;

public record UpdateLocationRequest(
    string Name,
    string TimeZone,
    AddressDto Adress);