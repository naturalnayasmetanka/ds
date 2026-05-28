using DS.Contracts.Departments.Update;
using FluentValidation;

namespace DS.Application.Departments.Validations
{
    public class UpdateDepartmentsValidator : AbstractValidator<UpdateDepartmentRequest>
    {
        public UpdateDepartmentsValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Name cannot be null")
               .MinimumLength(3).WithMessage("Name min length 3")
               .MaximumLength(100).WithMessage("Name max length 100");

            RuleFor(x => x.Slug)
               .NotEmpty().WithMessage("Slug cannot be null")
               .MinimumLength(1).WithMessage("Slug min length 1")
               .MaximumLength(100).WithMessage("Slug max length 100");
        }
    }
}
