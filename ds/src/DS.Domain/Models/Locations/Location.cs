using CSharpFunctionalExtensions;

namespace DS.Domain.Models.Locations;

public class Location
{
    //ef
    private Location() { }

    private Location(
        Guid id,
        Name name,
        Address address,
        Timezone timezone)
    {
        Id = id;
        Name = name;
        Address = address;
        Timezone = timezone;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public Name Name { get; private set; }
    public Address Address { get; private set; }
    public Timezone Timezone { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public static Result<Location, string> Create(
        Guid id,
        Name name,
        Address adress,
        Timezone timezone)
    {
        return Result.Success<Location, string>(new Location(
            id,
            name,
            adress,
            timezone));
    }

    public static Result<Location, string> Update(
        Location existLocaiton,
        Name name,
        Address adress,
        Timezone timezone)
    {
        existLocaiton.Name = name;
        existLocaiton.Address = adress;
        existLocaiton.Timezone = timezone;

        return Result.Success<Location, string>(existLocaiton);
    }
}
