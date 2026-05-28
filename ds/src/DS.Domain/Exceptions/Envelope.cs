using System.Text.Json.Serialization;

namespace DS.Domain.Exceptions;

public record Envelope
{
    public object? Result { get; }
    public Errors? ErrorList { get; }
    public bool IsError => ErrorList is not null || (ErrorList is not null && ErrorList.Any());
    public DateTime TimeGenerated { get; }

    [JsonConstructor]
    public Envelope(object? result, Errors? errorList)
    {
        Result = result;
        ErrorList = errorList;
        TimeGenerated = DateTime.Now;
    }

    public static Envelope Ok(object? result = null) => new(result, null);

    public static Envelope Error(Errors errors) => new(null, errors);
}

public record Envelope<T>
{
    public T? Result { get; }
    public Errors? ErrorList { get; }
    public bool IsError => ErrorList is not null || (ErrorList is not null && ErrorList.Any());
    public DateTime TimeGenerated { get; }

    [JsonConstructor]
    public Envelope(T? result, Errors? errorList)
    {
        Result = result;
        ErrorList = errorList;
        TimeGenerated = DateTime.Now;
    }

    public static Envelope Ok(T? result = default) => new(result, null);

    public static Envelope Error(Errors errors) => new(null, errors);
}

