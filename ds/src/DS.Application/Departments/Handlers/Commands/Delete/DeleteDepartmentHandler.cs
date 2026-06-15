using CSharpFunctionalExtensions;
using DS.Application.Abstractions.Database;
using DS.Application.Abstractions.Handlers;
using DS.Application.Departments.Repositories;
using DS.Domain.Exceptions;

namespace DS.Application.Departments.Handlers.Commands.Delete
{
    public class DeleteDepartmentHandler : ICommandHandler<DeleteDepartmentCommand>
    {
        private readonly IDepartmentsRepository _departmentsRepository;
        private readonly ITransactionManager _transactionManager;
        public DeleteDepartmentHandler(
            IDepartmentsRepository departmentsRepository,
            ITransactionManager transactionManager)
        {
            _departmentsRepository = departmentsRepository;
            _transactionManager = transactionManager;
        }

        public async Task<UnitResult<Errors>> Handle(
            DeleteDepartmentCommand command,
            CancellationToken cancellationToken = default)
        {
            var departmentResult = await _departmentsRepository.GetByFieldAsync(x => x.Id == command.Id && x.IsActive, cancellationToken);

            if (departmentResult.IsFailure)
                return UnitResult.Failure<Errors>(Error.Failure("department.get.failure", "Ошибка получения подразделения"));

            var department = departmentResult.Value;

            if (department is null)
                return UnitResult.Failure<Errors>(Error.NotFound("department.not.found", "Подразделение не найдено", command.Id));

            department.Deactivate();

            var saveResult = await _transactionManager.SaveChangesAsync(cancellationToken);

            if (saveResult.IsFailure)
                return UnitResult.Failure<Errors>(Error.Failure("save.failure", "Ошибка сохранения"));

            return UnitResult.Success<Errors>();
        }
    }
}
