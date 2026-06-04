using CSharpFunctionalExtensions;
using DS.Domain.Exceptions;

namespace DS.Application.Abstractions;

public interface IQueryHandler<TResponse, in TQuery> where TQuery : IQuery
{
    Task<Result<TResponse, Errors>> Handle(TQuery query, CancellationToken cancellationToken = default);
}

public interface IQueryHandler<in TQuery> where TQuery : IQuery
{
    Task<UnitResult<Errors>> Handle(TQuery query, CancellationToken cancellationToken = default);
}