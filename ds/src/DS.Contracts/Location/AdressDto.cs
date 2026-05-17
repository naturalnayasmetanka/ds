namespace DS.Contracts.Location;

public record AddressDto(
        string Country,
        string Region,
        string SettlementName,
        string SettlementType,
        string Street,
        string BuildingNumber,
        string? BuildingBlock,
        string Entrance,
        string Floor,
        string PremiseNumber,
        string PremiseType,
        string PostCode,
        string FullAddress,
        string? Comment);
