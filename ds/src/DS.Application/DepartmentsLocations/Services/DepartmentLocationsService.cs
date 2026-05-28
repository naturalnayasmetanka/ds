using CSharpFunctionalExtensions;
using DS.Application.Departments.Repositories;
using DS.Application.DepartmentsLocations.Repositories;
using DS.Application.Locations.Repositories;
using DS.Contracts.DepartmentsLocations.Bind;
using DS.Contracts.DepartmentsLocations.Unbind;
using DS.Domain.Exceptions;
using DS.Domain.Models.DepartmentsLocations;
using Microsoft.Extensions.Logging;

namespace DS.Application.DepartmentsLocations.Services;

public class DepartmentLocationsService : IDepartmentLocationsService
{
    private readonly IDepartmentsLocationsRepository _departmentsLocationsRepository;
    private readonly IDepartmentsRepository _departmentsRepository;
    private readonly ILocationsRepository _locationsRepository;
    private readonly ILogger<DepartmentLocationsService> _logger;

    public DepartmentLocationsService(
        IDepartmentsLocationsRepository departmentsLocationsRepository,
        IDepartmentsRepository departmentsRepository,
        ILocationsRepository locationsRepository,
        ILogger<DepartmentLocationsService> logger)
    {
        _departmentsLocationsRepository = departmentsLocationsRepository;
        _departmentsRepository = departmentsRepository;
        _locationsRepository = locationsRepository;
        _logger = logger;
    }

    public async Task<UnitResult<Errors>> BindAsync(
        BindDepartmentLocationRequest request,
        CancellationToken cancellationToken)
    {
        var department = await _departmentsRepository
            .GetByFieldAsync(x => x.Id == request.departmentId, cancellationToken);

        if (department.Value is null)
            return UnitResult.Failure<Errors>(Error.Failure("department.not.found", "Подразделение не найдено"));

        var location = await _locationsRepository
            .GetByFieldAsync(x => x.Id == request.locationId, cancellationToken);

        if (location.Value is null)
            return UnitResult.Failure<Errors>(Error.Failure("location.not.found", "Расположение не найдено"));

        var departmentLocation = await _departmentsLocationsRepository
            .GetByIdsAsync(DepartmentLocation.Create(request.departmentId, request.locationId).Value, cancellationToken);

        if (departmentLocation.Value is not null)
            return UnitResult.Failure<Errors>(Error.Failure("department.location.already.exists", "Подразделение с таким расположением уже существует"));

        await _departmentsLocationsRepository
            .BindAsync(DepartmentLocation.Create(request.departmentId, request.locationId).Value, cancellationToken);

        await _departmentsLocationsRepository.SaveAsync(cancellationToken);

        return UnitResult.Success<Errors>();
    }

    public async Task<UnitResult<Errors>> UnbindAsync(
        UnbindDepartmentLocationRequest request,
        CancellationToken cancellationToken)
    {
        var department = await _departmentsRepository.
            GetByFieldAsync(x => x.Id == request.departmentId, cancellationToken);

        if (department.Value is null)
            return UnitResult.Failure<Errors>(Error.Failure("department.not.found", "Подразделение не найдено"));

        var location = await _locationsRepository
            .GetByFieldAsync(x => x.Id == request.locationId, cancellationToken);

        if (location.Value is null)
            return UnitResult.Failure<Errors>(Error.Failure("location.not.found", "Расположение не найдено"));

        var departmentLocation = await _departmentsLocationsRepository
            .GetByIdsAsync(DepartmentLocation.Create(request.departmentId, request.locationId).Value, cancellationToken);

        if (departmentLocation.Value is not null)
            return UnitResult.Failure<Errors>(Error.Failure("department.location.already.exists", "Подразделение с таким расположением уже существует"));

        _departmentsLocationsRepository
            .Unbind(DepartmentLocation.Create(request.departmentId, request.locationId).Value, cancellationToken);

        await _departmentsLocationsRepository.SaveAsync(cancellationToken);

        return UnitResult.Success<Errors>();
    }
}
