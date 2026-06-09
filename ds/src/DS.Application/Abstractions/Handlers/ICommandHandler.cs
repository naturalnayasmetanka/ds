using CSharpFunctionalExtensions;
using DS.Domain.Exceptions;

namespace DS.Application.Abstractions.Handlers;

public interface ICommandHandler<TResponse, in TCommand> where TCommand : ICommand
{
    Task<Result<TResponse, Errors>> Handle(TCommand command, CancellationToken cancellationToken = default);
}

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task<UnitResult<Errors>> Handle(TCommand command, CancellationToken cancellationToken = default);
}