using CSharpFunctionalExtensions;
using DS.Application.Abstractions.Database;
using DS.Application.Abstractions.Handlers;
using DS.Application.Departments.Repositories;
using DS.Application.DepartmentsPositions.Repositories;
using DS.Application.Positions.Repositories;
using DS.Domain.Exceptions;
using DS.Domain.Models.DepartmentsPositions;
using Microsoft.Extensions.Logging;

namespace DS.Application.DepartmentsPositions.Handlers.Bind;

public class BindDepartmentPositionHandler : ICommandHandler<BindDepartmentPositionCommand>
{
    private readonly IDepartmentsPositionsRepository _departmentsPositionsRepository;
    private readonly IDepartmentsRepository _departmentsRepository;
    private readonly IPositionsRepository _positionsRepository;
    private readonly ITransactionManager _transactionManager;
    private readonly ILogger<BindDepartmentPositionHandler> _logger;

    public BindDepartmentPositionHandler(
        IDepartmentsPositionsRepository departmentsPositionsRepository,
        IDepartmentsRepository departmentsRepository,
        IPositionsRepository positionsRepository,
        ITransactionManager transactionManager,
        ILogger<BindDepartmentPositionHandler> logger)
    {
        _departmentsPositionsRepository = departmentsPositionsRepository;
        _departmentsRepository = departmentsRepository;
        _positionsRepository = positionsRepository;
        _transactionManager = transactionManager;
        _logger = logger;
    }

    public async Task<UnitResult<Errors>> Handle(BindDepartmentPositionCommand command, CancellationToken cancellationToken = default)
    {
        var transactionScopeResult = await _transactionManager.BeginTransactionAsync(cancellationToken);

        if (transactionScopeResult.IsFailure)
            return UnitResult.Failure<Errors>(Error.Failure("transaction.failure", "Ошибка транзакции"));

        using var transactionScope = transactionScopeResult.Value;

        var existDepartment = await _departmentsRepository.GetByFieldAsync(x => x.Id == command.request.DepartmentId, cancellationToken);
        if (existDepartment.Value is null)
            return UnitResult.Failure<Errors>(Error.Failure("department.not.found", "Подразделение не найдено"));

        var existPosition = await _positionsRepository.GetByFieldAsync(x => x.Id == command.request.PositionId, cancellationToken);
        if (existPosition.Value is null)
            return UnitResult.Failure<Errors>(Error.Failure("position.not.found", "Должность не найдена"));

        var alreadyBind = await _departmentsPositionsRepository.GetByFieldAsync(x => x.DepartmentId == command.request.DepartmentId && x.PositionId == command.request.PositionId, cancellationToken);
        if (alreadyBind.Value is not null)
            return UnitResult.Failure<Errors>(Error.Failure("department.position.already.exists", "Связь уже существует"));

        var newBind = new DepartmentPosition(command.request.DepartmentId, command.request.PositionId);

        var addResult = await _departmentsPositionsRepository.AddAsync(newBind, cancellationToken);

        if (addResult.IsFailure)
            return UnitResult.Failure<Errors>(Error.Failure("department.position.add.error", "Ошибка добавления"));

        var saveResult = await _transactionManager.SaveChangesAsync(cancellationToken);

        if (saveResult.IsFailure)
            return UnitResult.Failure<Errors>(Error.Failure("save.failure", "Ошибка сохранения"));

        return UnitResult.Success<Errors>();
    }
}
