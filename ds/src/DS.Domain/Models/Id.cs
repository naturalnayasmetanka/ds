using CSharpFunctionalExtensions;

namespace DS.Domain.Models;

public class Id
{
    private Id() { }

    public Guid Value { get; }

    public static Result<Guid> Create() => Result.Success<Guid>(Guid.CreateVersion7());
}
