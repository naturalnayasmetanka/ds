using DS.Application.Locations.Validations.CustomRules;
using DS.Contracts.Locations;
using DS.Contracts.Locations.Create;
using FluentValidation;

namespace DS.Application.Locations.Validations;

public class CreateLocationsValidator : AbstractValidator<CreateLocationRequest>
{
    public CreateLocationsValidator(IValidator<AddressDto> addressValidator)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name cannot be null")
            .MinimumLength(3).WithMessage("Name min length 3")
            .MaximumLength(100).WithMessage("Name max length 100");

        RuleFor(x => x.TimeZone)
            .NotEmpty().WithMessage("TimeZone cannot be null")
            .MaximumLength(50)
            .Must(TimeZoneRules.BeValidTimeZoneId)
            .WithMessage("Invalid time zone identifier is specified");

        RuleFor(x => x.Adress)
            .NotNull().WithMessage("Address cannot be null")
            .SetValidator(addressValidator);
    }
}

public class AddressDtoValidator : AbstractValidator<AddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country cannot be null")
            .MinimumLength(2).WithMessage("Country min length 2")
            .MaximumLength(100).WithMessage("Country max length 100");

        RuleFor(x => x.Region)
            .NotEmpty().WithMessage("Region cannot be null")
            .MaximumLength(100).WithMessage("Region max length 100");

        RuleFor(x => x.SettlementName)
            .NotEmpty().WithMessage("Settlement name cannot be null")
            .MinimumLength(2).WithMessage("Settlement name min length 2")
            .MaximumLength(100).WithMessage("Premise type max length 100");

        RuleFor(x => x.SettlementType)
            .NotEmpty().WithMessage("Settlement type cannot be null")
            .MaximumLength(50).WithMessage("Settlement type max length 50");

        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Street cannot be null")
            .MinimumLength(3).WithMessage("Street min length 3")
            .MaximumLength(150).WithMessage("Street max length 150");

        RuleFor(x => x.BuildingNumber)
            .NotEmpty().WithMessage("Building number cannot be null")
            .MaximumLength(20).WithMessage("Building number max length 20")
            .Matches(@"^[0-9A-Za-zА-Яа-я\/\- ]+$")
            .WithMessage("Building number can contain only numbers, letters (rus/eng), /, -, and a space.");

        RuleFor(x => x.BuildingBlock)
            .MaximumLength(20).WithMessage("Building block max length 20")
            .Matches(@"^[0-9A-Za-zА-Яа-я\/\- ]+$")
            .WithMessage("Building block can contain only numbers, letters (rus/eng), /, -, and a space.");

        RuleFor(x => x.Entrance)
            .NotEmpty().WithMessage("Entrance cannot be null")
            .MaximumLength(10).WithMessage("Entrance max length 10")
            .Matches(@"^[0-9A-Za-zА-Яа-я\/\- ]+$")
            .WithMessage("Entrance can contain only numbers, letters (rus/eng), /, -, and a space.");

        RuleFor(x => x.Floor)
            .NotEmpty().WithMessage("Floor cannot be null")
            .MaximumLength(10).WithMessage("Floor max length 10")
            .Matches(@"^[0-9A-Za-zА-Яа-я\/\- ]+$")
            .WithMessage("Floor can contain only numbers, letters (rus/eng), /, -, and a space.");

        RuleFor(x => x.PremiseNumber)
            .NotEmpty().WithMessage("Premise number cannot be null")
            .MaximumLength(20).WithMessage("Premise number max length 20");

        RuleFor(x => x.PremiseType)
            .NotEmpty().WithMessage("Premise type cannot be null")
            .MaximumLength(30).WithMessage("Premise type max length 30");

        RuleFor(x => x.PostCode)
            .NotEmpty().WithMessage("Post code cannot be null")
            .Matches(@"^\d{5,6}$").WithMessage("Post code must contain 5 or 6 digits");

        RuleFor(x => x.FullAddress)
            .NotEmpty().WithMessage("Full address cannot be null")
            .MaximumLength(500).WithMessage("Full address max length 500");

        RuleFor(x => x.Comment)
            .MaximumLength(500).WithMessage("Comment max length 500");
    }
}
