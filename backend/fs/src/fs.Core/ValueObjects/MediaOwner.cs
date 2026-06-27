using CSharpFunctionalExtensions;
using FS.Core.Exceptions;

namespace FS.Core.ValueObjects;

public sealed record MediaOwner
{
    private static readonly HashSet<string> AllowedContexts =
    [
        "lesson",
        "module",
        "user",
    ];

    public string Context { get; }

    public Guid EntityId { get; }

    protected MediaOwner()
    {

    }

    private MediaOwner(string context, Guid entityId)
    {
        Context = context;
        EntityId = entityId;
    }


    public static Result<MediaOwner, Error> Create(string context, Guid entityId)
    {
        if (string.IsNullOrWhiteSpace(context))
            return Result.Failure<MediaOwner, Error>(Error.Failure("empty.context", "connot be empty"));

        if (context.Length > 50)
            return Result.Failure<MediaOwner, Error>(Error.Failure("Length.context", "cant be > 50"));

        string normalizedContext = context.Trim().ToLowerInvariant();
        if (!AllowedContexts.Contains(normalizedContext))
            return Result.Failure<MediaOwner, Error>(Error.Failure("unnormalized.context", "unnormalized.context"));

        if (entityId == Guid.Empty)
            return Result.Failure<MediaOwner, Error>(Error.Failure("empty.guid", "empty.guid"));

        return Result.Success<MediaOwner, Error>(new MediaOwner(normalizedContext, entityId));
    }
}