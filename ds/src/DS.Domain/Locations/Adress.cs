using CSharpFunctionalExtensions;
using DS.Domain.Attributes;
using DS.Domain.Extentions;

namespace DS.Domain.Locations;

public record Adress
{
    private Adress(
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

    public static Result<Adress, List<string>> Create(
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
        var adress = new Adress(
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

        List<string> errors = Validator.ValidateRequired(adress);

        if (errors.Any())
            return Result.Failure<Adress, List<string>>(errors);

        return Result.Success<Adress, List<string>>(adress);
    }
}
