namespace DS.Domain.Models;

public class Id
{
    private Id() { }

    public Guid Value { get; }

    public static Guid Create() => Guid.CreateVersion7();
}
