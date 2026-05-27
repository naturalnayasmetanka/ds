using DS.Domain.Enums;

namespace DS.Domain.Exceptions;

public record Error
{
    public string Code { get; }
    public string Message { get; }
    public string? InvalidField { get; }
    public ErrorType ErrorType { get; }

    private Error(
        string code,
        string message,
        ErrorType errorType,
        string? invalidField = null)
    {
        Code = code;
        Message = message;
        ErrorType = errorType;
        InvalidField = invalidField;
    }

    public static Error NotFound(string? code, string message, Guid? id)
        => new(code ?? "record.not.found", message, ErrorType.NotFound);
    public static Error Validation(string? code, string message, string? invalidField = null)
        => new(code ?? "value.is.invalid", message, ErrorType.Validation, invalidField);
    public static Error Conflict(string? code, string message)
        => new(code ?? "value.is.conflict", message, ErrorType.Conflict);
    public static Error Failure(string? code, string message)
        => new(code ?? "failure", message, ErrorType.Failure);
}