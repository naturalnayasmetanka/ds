using DS.Contracts.Departments.Create;
using DS.Domain.Models.Departments;
using DS.Domain.Validation;
using FluentValidation;

namespace DS.Application.Departments.Handlers.Create
{
    public class CreateDepartmentsValidator : AbstractValidator<CreateDepartmentRequest>
    {
        public CreateDepartmentsValidator()
        {
            RuleFor(x => x.Name)
                .MustBeValueObject(Name.Create);
        }
    }
}
