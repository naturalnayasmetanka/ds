using CSharpFunctionalExtensions;
using FS.Core.Exceptions;

namespace FS.Core.ValueObjects;

public sealed record FileName
{
    public string Name { get; }
    public string Extention { get; }

    private FileName(string name, string extention)
    {
        Name = name;
        Extention = extention;
    }

    public static Result<FileName, Error> Create(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return Result.Failure<FileName, Error>(Error.Failure("empty.filename", "cannot be empty"));

        int lastDot = fileName.LastIndexOf('.');
        if (lastDot <= 0 || lastDot == fileName.Length - 1)
            return Result.Failure<FileName, Error>(Error.Failure("empty.extension", "must have extension"));

        string name = fileName[..lastDot];
        string extension = fileName[(lastDot + 1)..].ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<FileName, Error>(Error.Failure("empty.filename", "file name cannot be empty"));

        return Result.Success<FileName, Error>(new FileName(fileName, extension));
    }
}
