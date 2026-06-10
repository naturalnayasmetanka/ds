using DS.Contracts.Locations;
using DS.Contracts.Locations.Create;
using DS.Domain.Models.Locations;
using DS.Domain.Validation;
using FluentValidation;

namespace DS.Application.Locations.Handlers.Commands.Create;

public class CreateLocationsValidator : AbstractValidator<CreateLocationRequest>
{
    public CreateLocationsValidator(IValidator<AddressDto> addressValidator)
    {
        RuleFor(x => x.Name)
            .MustBeValueObject(Name.Create);

        RuleFor(x => x.TimeZone)
           .MustBeValueObject(Timezone.Create);

        RuleFor(x => x.Adress)
            .NotNull().WithMessage("Address cannot be null")
            .SetValidator(addressValidator);
    }
}
