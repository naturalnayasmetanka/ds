using DS.Application.Departments.Repositories;
using DS.Application.DepartmentsLocations.Repositories;
using DS.Application.Locations.Repositories;
using DS.Contracts.Departments.Create;
using DS.Contracts.Departments.Update;
using DS.Domain.Models;
using DS.Domain.Models.Departments;
using DS.Domain.Models.DepartmentsLocations;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Path = DS.Domain.Models.Departments.Path;

namespace DS.Application.Departments.Services;

public class DepartmentsService : IDepartmantsService
{
    private readonly IDepartmentsRepository _departmentsRepository;
    private readonly ILocationsRepository _locationsRepository;
    private readonly IDepartmentsLocationsRepository _departmentsLocationsRepository;
    private readonly IValidator<CreateDepartmentRequest> _createDepartmentRequetValidator;
    private readonly IValidator<UpdateDepartmentRequest> _updateDepartmentRequetValidator;

    private readonly ILogger<DepartmentsService> _logger;

    public DepartmentsService(
        ILocationsRepository locationsRepository,
        IDepartmentsRepository departmentsRepository,
        IDepartmentsLocationsRepository departmentsLocationsRepository,
        IValidator<CreateDepartmentRequest> createDepartmentRequetValidator,
        IValidator<UpdateDepartmentRequest> updateDepartmentRequetValidator,
        ILogger<DepartmentsService> logger)
    {
        _locationsRepository = locationsRepository;
        _departmentsRepository = departmentsRepository;
        _departmentsLocationsRepository = departmentsLocationsRepository;
        _createDepartmentRequetValidator = createDepartmentRequetValidator;
        _updateDepartmentRequetValidator = updateDepartmentRequetValidator;
        _logger = logger;
    }

    public async Task<Guid> CreateAsync(
        CreateDepartmentRequest request,
        CancellationToken cancellationToken)
    {
        var fullValidationResult =
            await _createDepartmentRequetValidator.ValidateAsync(request);

        if (!fullValidationResult.IsValid)
            throw new ValidationException(fullValidationResult.Errors);

        if (request.Locations is not null)
        {
            var existLocations = await _locationsRepository
                .AllLocationsExistAsync(request.Locations, cancellationToken);

            if (!existLocations)
                throw new Exception("Invalid locations");
        }

        var departmentId = Id.Create();

        var existParentDepartment = await _departmentsRepository
            .GetByFieldAsync(x => x.Id == request.ParentId, cancellationToken);

        var path = existParentDepartment is null ?
            Path.Create(request.Slug).Value :
            Path.Create(existParentDepartment.Path.Value, request.Slug).Value;

        var newDepartment = Department.Create(
            departmentId,
            Name.Create(request.Name).Value,
            path,
            Identifier.Create(request.Slug).Value,
            request.ParentId,
            0,
            0).Value;

        List<DepartmentLocation> newDepartmentsLocations = request.Locations
            .Select(locationId => DepartmentLocation.Create(newDepartment.Id, locationId)).ToList();

        var createDepartmentResult = await _departmentsRepository
            .AddAsync(newDepartment, cancellationToken);

        await _departmentsLocationsRepository.AddRangeAsync(newDepartmentsLocations, cancellationToken);

        await _departmentsRepository.SaveAsync(cancellationToken);

        return createDepartmentResult;
    }

    public async Task<Guid> UpdateAsync(
        Guid departmentId,
        UpdateDepartmentRequest request,
        CancellationToken cancellationToken)
    {
        var fullValidationResult =
            await _updateDepartmentRequetValidator.ValidateAsync(request);

        if (!fullValidationResult.IsValid)
            throw new ValidationException(fullValidationResult.Errors);

        var existDepartment =
           await _departmentsRepository.GetByFieldAsync(x => x.Id == departmentId, cancellationToken);

        if (existDepartment is null)
            throw new Exception("Department not found");

        var updatedDepartment =
            Department.Update(
                existDepartment,
                Name.Create(request.Name).Value,
                Identifier.Create(request.Slug).Value);

        await _departmentsRepository.SaveAsync(cancellationToken);

        return updatedDepartment.Value.Id;
    }
}
