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
            return Result.Failure<FileName, Error>(Error.Failure("empty.filename", "connot be empty"));

        int lastDot = fileName.LastIndexOf('.');
        if (lastDot == -1 || lastDot == fileName.Length - 1)
            return Result.Failure<FileName, Error>(Error.Failure("empty.extention", "must have extention"));

        string extention = fileName[(lastDot + 1)..].ToLowerInvariant();

        return Result.Success<FileName, Error>(new FileName(fileName, extention));
    }
}
