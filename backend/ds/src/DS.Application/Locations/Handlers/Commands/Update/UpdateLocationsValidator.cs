using DS.Application.Locations.Validations.CustomRules;
using DS.Contracts.Locations;
using DS.Contracts.Locations.Update;
using DS.Domain.Models.Locations;
using DS.Domain.Validation;
using FluentValidation;

namespace DS.Application.Locations.Handlers.Commands.Update;

public class UpdateLocationsValidator : AbstractValidator<UpdateLocationRequest>
{
    public UpdateLocationsValidator(IValidator<AddressDto> addressValidator)
    {
        RuleFor(x => x.Name)
             .MustBeValueObject(Name.Create);

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
