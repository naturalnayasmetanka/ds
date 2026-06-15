using CSharpFunctionalExtensions;
using DS.Domain.Exceptions;

namespace DS.Application.Abstractions.Database;

public interface ITransactionScope : IDisposable
{
    UnitResult<Errors> Commit();

}