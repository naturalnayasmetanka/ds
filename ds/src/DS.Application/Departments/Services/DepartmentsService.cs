using CSharpFunctionalExtensions;
using DS.Application.Departments.Exceptions;
using DS.Application.Departments.Repositories;
using DS.Application.DepartmentsLocations.Repositories;
using DS.Application.Extentions;
using DS.Application.Locations.Repositories;
using DS.Contracts.Departments.Create;
using DS.Contracts.Departments.Update;
using DS.Domain.Exceptions;
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

    public async Task<Result<Guid, List<Error>>> CreateAsync(
        CreateDepartmentRequest request,
        CancellationToken cancellationToken)
    {
        var fullValidationResult =
            await _createDepartmentRequetValidator.ValidateAsync(request);

        if (!fullValidationResult.IsValid)
            return Result.Failure<Guid, List<Error>>(fullValidationResult.ToErrorList());

        var existLocations = await _locationsRepository
            .AllLocationsExistAsync(request.Locations, cancellationToken);

        if (!existLocations.Value)
            return Result.Failure<Guid, List<Error>>(new List<Error>() { Error.Failure("location.not.found", "Локация не найдена") });

        var departmentId = Id.Create();

        var existParentDepartment = await _departmentsRepository
            .GetByFieldAsync(x => x.Id == request.ParentId, cancellationToken);

        var path = existParentDepartment.Value is null ?
            Path.Create(request.Slug).Value :
            Path.Create(existParentDepartment.Value.Path.Value, request.Slug).Value;

        var newDepartment = Department.Create(
            departmentId.Value,
            Name.Create(request.Name).Value,
            path,
            Identifier.Create(request.Slug).Value,
            request.ParentId,
            0,
            0).Value;

        var newDepartmentsLocations = request.Locations
            .Select(locationId => DepartmentLocation.Create(newDepartment.Id, locationId).Value).ToList();

        var createDepartmentResult = await _departmentsRepository
            .AddAsync(newDepartment, cancellationToken);

        await _departmentsLocationsRepository.AddRangeAsync(newDepartmentsLocations, cancellationToken);

        await _departmentsRepository.SaveAsync(cancellationToken);

        return Result.Success<Guid, List<Error>>(createDepartmentResult.Value);
    }

    public async Task<Result<Guid, List<Error>>> UpdateAsync(
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

        if (existDepartment.Value is null)
            throw new DepartmentNotFoundException(Error.Failure("department.not.found", "Подразделение не найдено"));

        var updatedDepartment =
            Department.Update(
                existDepartment.Value,
                Name.Create(request.Name).Value,
                Identifier.Create(request.Slug).Value);

        await _departmentsRepository.SaveAsync(cancellationToken);

        return Result.Success<Guid, List<Error>>(updatedDepartment.Value.Id);
    }
}
