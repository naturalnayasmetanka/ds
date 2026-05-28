using CSharpFunctionalExtensions;
using DS.Domain.Exceptions;

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

    public static Result<Location, Errors> Create(Guid id, Name name, Address adress, Timezone timezone)
    {
        if (string.IsNullOrEmpty(name.Value))
        {
            return Result.Failure<Location, Errors>(Error.Validation("name.invalid", "Name cannot be null", "Name"));
        }

        return Result.Success<Location, Errors>(new Location(id, name, adress, timezone));
    }


    public static Result<Location, Errors> Update(Location existLocaiton, Name name, Address address, Timezone timezone)
    {
        if (string.IsNullOrEmpty(name.Value))
        {
            return Result.Failure<Location, Errors>(Error.Validation("name.invalid", "Name cannot be null", "Name"));
        }

        existLocaiton.Name = name;
        existLocaiton.Address = address;
        existLocaiton.Timezone = timezone;

        return Result.Success<Location, Errors>(existLocaiton);
    }
}
