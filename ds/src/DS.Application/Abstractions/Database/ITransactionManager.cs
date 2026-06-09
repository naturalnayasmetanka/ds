using CSharpFunctionalExtensions;
using DS.Domain.Exceptions;

namespace DS.Application.Abstractions.Database;

public interface ITransactionManager
{
    Task<Result<ITransactionScope, Errors>> BeginTransactionAsync(CancellationToken cancellationToken);
    Task<UnitResult<Errors>> SaveChangesAsync(CancellationToken cancellationToken);
}