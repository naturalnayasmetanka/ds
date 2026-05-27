using System.Text.Json.Serialization;

namespace DS.Domain.Exceptions;

public record Envelope
{
    public object? Result { get; }
    public List<Error> ErrorList { get; }
    public bool IsError => ErrorList is not null || (ErrorList is not null && ErrorList.Any());
    public DateTime TimeGenerated { get; }

    [JsonConstructor]
    public Envelope(object? result, List<Error> errorList)
    {
        Result = result;
        ErrorList = errorList;
        TimeGenerated = DateTime.Now;
    }

    public static Envelope Ok(object? result = null) => new(result, null);

    public static Envelope Error(List<Error> errors) => new(null, errors);
}

public record Envelope<T>
{
    public T? Result { get; }
    public List<Error> ErrorList { get; }
    public bool IsError => ErrorList is not null || (ErrorList is not null && ErrorList.Any());
    public DateTime TimeGenerated { get; }

    [JsonConstructor]
    public Envelope(T? result, List<Error> errorList)
    {
        Result = result;
        ErrorList = errorList;
        TimeGenerated = DateTime.Now;
    }

    public static Envelope Ok(T? result = default) => new(result, null);

    public static Envelope Error(List<Error> errors) => new(null, errors);
}

