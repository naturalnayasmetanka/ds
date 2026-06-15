using CSharpFunctionalExtensions;
using DS.Domain.Attributes;

namespace DS.Domain.Models.Locations;

public record Address
{
    private Address(
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
        string? comment
       )
    {
        Country = country;
        Region = region;
        SettlementName = settlementName;
        SettlementType = settlementType;
        Street = street;
        BuildingNumber = buildingNumber;
        BuildingBlock = buildingBlock;
        Entrance = entrance;
        Floor = floor;
        PremiseNumber = premiseNumber;
        PremiseType = premiseType;
        PostCode = postCode;
        FullAddress = fullAddress;
        Comment = comment;
    }

    [NotEmpty]
    public string Country { get; }
    [NotEmpty]
    public string Region { get; }
    [NotEmpty]
    public string SettlementName { get; }
    [NotEmpty]
    public string SettlementType { get; }
    [NotEmpty]
    public string Street { get; }
    [NotEmpty]
    public string BuildingNumber { get; }

    public string? BuildingBlock { get; }
    [NotEmpty]
    public string Entrance { get; }
    [NotEmpty]
    public string Floor { get; set; }
    [NotEmpty]
    public string PremiseNumber { get; }
    [NotEmpty]
    public string PremiseType { get; }
    [NotEmpty]
    public string PostCode { get; }
    [NotEmpty]
    public string FullAddress { get; }
    public string? Comment { get; }

    public static Result<Address> Create(
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
        string? comment)
    {
        var adress = new Address(
               country,
               region,
               settlementName,
               settlementType,
               street,
               buildingNumber,
               buildingBlock,
               entrance,
               floor,
               premiseNumber,
               premiseType,
               postCode,
               fullAddress,
               comment);

        return Result.Success<Address>(adress);
    }
}
