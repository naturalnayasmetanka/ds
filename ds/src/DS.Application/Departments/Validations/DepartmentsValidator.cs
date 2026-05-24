using DS.Contracts.Department.Create;
using FluentValidation;

namespace DS.Application.Departments.Validations
{
    public class DepartmentsValidator : AbstractValidator<CreateDepartmentRequest>
    {
        public DepartmentsValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Name cannot be null")
               .MinimumLength(3).WithMessage("Name min length 3")
               .MaximumLength(100).WithMessage("Name max length 100");
        }
    }
}
