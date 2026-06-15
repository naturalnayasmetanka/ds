using CSharpFunctionalExtensions;
using DS.Application.Abstractions.Database;
using DS.Application.Abstractions.Handlers;
using DS.Application.DepartmentsPositions.Repositories;
using DS.Contracts.DepartmentsPositions.Unbind;
using DS.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace DS.Application.DepartmentsPositions.Handlers.Unbind;

public class UnbindDepartmentPositionHandler : ICommandHandler<UnbindDepartmentPositionCommand>
{
    private readonly IDepartmentsPositionsRepository _departmentsPositionsRepository;
    private readonly ITransactionManager _transactionManager;
    private readonly ILogger<UnbindDepartmentPositionHandler> _logger;

    public UnbindDepartmentPositionHandler(
        IDepartmentsPositionsRepository departmentsPositionsRepository,
        ITransactionManager transactionManager,
        ILogger<UnbindDepartmentPositionHandler> logger)
    {
        _departmentsPositionsRepository = departmentsPositionsRepository;
        _transactionManager = transactionManager;
        _logger = logger;
    }

    public async Task<UnitResult<Errors>> Handle(UnbindDepartmentPositionCommand command, CancellationToken cancellationToken = default)
    {
        var transactionScopeResult = await _transactionManager.BeginTransactionAsync(cancellationToken);

        if (transactionScopeResult.IsFailure)
            return UnitResult.Failure<Errors>(Error.Failure("transaction.failure", "Ошибка транзакции"));

        using var transactionScope = transactionScopeResult.Value;

        var existBind = await _departmentsPositionsRepository.GetByFieldAsync(x => x.DepartmentId == command.request.DepartmentId && x.PositionId == command.request.PositionId, cancellationToken);
        if (existBind.Value is null)
            return UnitResult.Failure<Errors>(Error.Failure("department.position.not.found", "Связь не найдена"));

        await _departmentsPositionsRepository.RemoveAsync(existBind.Value, cancellationToken);

        var saveResult = await _transactionManager.SaveChangesAsync(cancellationToken);

        if (saveResult.IsFailure)
            return UnitResult.Failure<Errors>(Error.Failure("save.failure", "Ошибка сохранения"));

        return UnitResult.Success<Errors>();
    }
}
