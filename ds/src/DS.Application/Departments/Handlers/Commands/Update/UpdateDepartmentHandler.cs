using CSharpFunctionalExtensions;
using DS.Application.Abstractions.Database;
using DS.Application.Abstractions.Handlers;
using DS.Application.Departments.Repositories;
using DS.Application.Extentions;
using DS.Contracts.Departments.Update;
using DS.Domain.Exceptions;
using DS.Domain.Models.Departments;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DS.Application.Departments.Handlers.Commands.Update
{
    public class UpdateDepartmentHandler : ICommandHandler<Guid, UpdateDepartmentCommand>
    {
        private readonly IDepartmentsRepository _departmentsRepository;
        private readonly ITransactionManager _transactionManager;
        private readonly IValidator<UpdateDepartmentRequest> _updateDepartmentRequetValidator;

        private readonly ILogger<UpdateDepartmentHandler> _logger;

        public UpdateDepartmentHandler(
            IDepartmentsRepository departmentsRepository,
            ITransactionManager transactionManager,
            IValidator<UpdateDepartmentRequest> updateDepartmentRequetValidator,
            ILogger<UpdateDepartmentHandler> logger)
        {
            _departmentsRepository = departmentsRepository;
            _transactionManager = transactionManager;
            _updateDepartmentRequetValidator = updateDepartmentRequetValidator;

            _logger = logger;
        }

        public async Task<Result<Guid, Errors>> Handle(
            UpdateDepartmentCommand command,
            CancellationToken cancellationToken = default)
        {
            var fullValidationResult = await _updateDepartmentRequetValidator.ValidateAsync(command.request);

            if (!fullValidationResult.IsValid)
                return Result.Failure<Guid, Errors>(fullValidationResult.ToErrorList());

            var existDepartment =
               await _departmentsRepository.GetByFieldAsync(x => x.Id == command.departmentId && x.IsActive, cancellationToken);

            if (existDepartment.Value is null)
                return Result.Failure<Guid, Errors>(Error.Failure("department.not.found", "Подразделение не найдена"));

            var updatedDepartment =
                Department.Update(
                    existDepartment.Value,
                    Name.Create(command.request.Name).Value,
                    Identifier.Create(command.request.Slug).Value);

            var saveResult = await _transactionManager.SaveChangesAsync(cancellationToken);

            if (saveResult.IsFailure)
                return Result.Failure<Guid, Errors>(Error.Failure("save.failure", "Ошибка сохранения"));

            return Result.Success<Guid, Errors>(updatedDepartment.Value.Id);
        }
    }
}
