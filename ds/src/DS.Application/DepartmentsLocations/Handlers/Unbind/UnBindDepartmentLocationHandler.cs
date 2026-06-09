using CSharpFunctionalExtensions;
using DS.Application.Abstractions.Database;
using DS.Application.Abstractions.Handlers;
using DS.Application.Departments.Repositories;
using DS.Application.DepartmentsLocations.Repositories;
using DS.Application.Locations.Repositories;
using DS.Domain.Exceptions;
using DS.Domain.Models.DepartmentsLocations;
using Microsoft.Extensions.Logging;

namespace DS.Application.DepartmentsLocations.Handlers.Unbind
{
    public class UnBindDepartmentLocationHandler : ICommandHandler<UnBindDepartmentLocationCommand>
    {
        private readonly IDepartmentsLocationsRepository _departmentsLocationsRepository;
        private readonly IDepartmentsRepository _departmentsRepository;
        private readonly ILocationsRepository _locationsRepository;
        private readonly ITransactionManager _transactionManager;
        private readonly ILogger<UnBindDepartmentLocationHandler> _logger;

        public UnBindDepartmentLocationHandler(
            IDepartmentsLocationsRepository departmentsLocationsRepository,
            IDepartmentsRepository departmentsRepository,
            ILocationsRepository locationsRepository,
            ITransactionManager transactionManager,
            ILogger<UnBindDepartmentLocationHandler> logger)
        {
            _departmentsLocationsRepository = departmentsLocationsRepository;
            _departmentsRepository = departmentsRepository;
            _locationsRepository = locationsRepository;
            _transactionManager = transactionManager;
            _logger = logger;
        }

        public async Task<UnitResult<Errors>> Handle(
            UnBindDepartmentLocationCommand command,
            CancellationToken cancellationToken = default)
        {
            var department = await _departmentsRepository.
           GetByFieldAsync(x => x.Id == command.request.departmentId, cancellationToken);

            if (department.Value is null)
                return UnitResult.Failure<Errors>(Error.Failure("department.not.found", "Подразделение не найдено"));

            var location = await _locationsRepository
                .GetByFieldAsync(x => x.Id == command.request.locationId, cancellationToken);

            if (location.Value is null)
                return UnitResult.Failure<Errors>(Error.Failure("location.not.found", "Расположение не найдено"));

            var departmentLocation = await _departmentsLocationsRepository
                .GetByIdsAsync(DepartmentLocation.Create(command.request.departmentId, command.request.locationId).Value, cancellationToken);

            if (departmentLocation.Value is not null)
                return UnitResult.Failure<Errors>(Error.Failure("department.location.already.exists", "Подразделение с таким расположением уже существует"));

            _departmentsLocationsRepository
                .Unbind(DepartmentLocation.Create(command.request.departmentId, command.request.locationId).Value, cancellationToken);

            await _transactionManager.SaveChangesAsync(cancellationToken);

            return UnitResult.Success<Errors>();
        }
    }
}
