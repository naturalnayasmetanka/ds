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
            var department = await _departmentsRepository.GetByFieldAsync(x => x.Id == command.Id, cancellationToken);

            if (department.Value is null)
                return Result.Failure<Guid, Errors>(Error.NotFound("department.not.found", "Подразделение не найдено", command.Id));

            department.Value.Deactivate();

            await _transactionManager.SaveChangesAsync(cancellationToken);

            return Result.Success<Errors>();
        }
    }
}
