using DS.Contracts.Departments.Create;
using DS.Domain.Models.Departments;
using DS.Domain.Validation;
using FluentValidation;

namespace DS.Application.Departments.Validations
{
    public class DepartmentsValidator : AbstractValidator<CreateDepartmentRequest>
    {
        public DepartmentsValidator()
        {
            RuleFor(x => x.Name)
                .MustBeValueObject(Name.Create);
        }
    }
}
