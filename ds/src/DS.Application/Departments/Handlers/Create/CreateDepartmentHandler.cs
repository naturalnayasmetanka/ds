using CSharpFunctionalExtensions;
using DS.Application.Abstractions;
using DS.Application.Departments.Repositories;
using DS.Application.DepartmentsLocations.Repositories;
using DS.Application.Extentions;
using DS.Application.Locations.Repositories;
using DS.Contracts.Departments.Create;
using DS.Domain.Exceptions;
using DS.Domain.Models;
using DS.Domain.Models.Departments;
using DS.Domain.Models.DepartmentsLocations;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Path = DS.Domain.Models.Departments.Path;

namespace DS.Application.Departments.Handlers.Create
{
    public class CreateDepartmentHandler : ICommandHandler<Guid, CreateDepartmentCommand>
    {
        private readonly IValidator<CreateDepartmentRequest> _createDepartmentRequetValidator;

        private readonly IDepartmentsRepository _departmentsRepository;
        private readonly ILocationsRepository _locationsRepository;
        private readonly IDepartmentsLocationsRepository _departmentsLocationsRepository;

        private readonly ILogger<CreateDepartmentHandler> _logger;

        public CreateDepartmentHandler(
            IValidator<CreateDepartmentRequest> createDepartmentRequetValidator,
            IDepartmentsRepository departmentsRepository,
            ILocationsRepository locationsRepository,
            IDepartmentsLocationsRepository departmentsLocationsRepository,
            ILogger<CreateDepartmentHandler> logger)
        {
            _createDepartmentRequetValidator = createDepartmentRequetValidator;
            _departmentsRepository = departmentsRepository;
            _locationsRepository = locationsRepository;
            _departmentsLocationsRepository = departmentsLocationsRepository;

            _logger = logger;
        }

        public async Task<Result<Guid, Errors>> Handle(
            CreateDepartmentCommand command,
            CancellationToken cancellationToken = default)
        {
            var fullValidationResult =
              await _createDepartmentRequetValidator.ValidateAsync(command.request);

            if (!fullValidationResult.IsValid)
                return Result.Failure<Guid, Errors>(fullValidationResult.ToErrorList());

            var existLocations = await _locationsRepository
                .AllLocationsExistAsync(command.request.Locations, cancellationToken);

            if (!existLocations.Value)
                return Result.Failure<Guid, Errors>(Error.Failure("location.not.found", "Локация не найдена"));

            var departmentId = Id.Create();

            var existParentDepartment = await _departmentsRepository
                .GetByFieldAsync(x => x.Id == command.request.ParentId, cancellationToken);

            var path = existParentDepartment.Value is null ?
                Path.Create(command.request.Slug).Value :
                Path.Create(existParentDepartment.Value.Path.Value, command.request.Slug).Value;

            var newDepartment = Department.Create(
                departmentId.Value,
                Name.Create(command.request.Name).Value,
                path,
                Identifier.Create(command.request.Slug).Value,
                command.request.ParentId,
                0,
                0).Value;

            var newDepartmentsLocations = command.request.Locations
                .Select(locationId => DepartmentLocation.Create(newDepartment.Id, locationId).Value).ToList();

            var createDepartmentResult = await _departmentsRepository
                .AddAsync(newDepartment, cancellationToken);

            await _departmentsLocationsRepository.AddRangeAsync(newDepartmentsLocations, cancellationToken);

            await _departmentsRepository.SaveAsync(cancellationToken);

            return Result.Success<Guid, Errors>(createDepartmentResult.Value);
        }
    }
}
