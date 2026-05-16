namespace DS.Contracts.Location.Create;

public record CreateLocationRequest(string Name, AddressDto Adress);

public record AddressDto(
        string country,
        string region,
        string settlementName,
        string settlementType,
        string street,
        string buildingNumber,
        string? buildingBlock,
        string entrance,
        string floor,
        string premiseNumber,
        string premiseType,
        string postCode,
        string fullAddress,
        string? comment);
