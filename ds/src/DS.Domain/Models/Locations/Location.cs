using CSharpFunctionalExtensions;
using DS.Domain.Models.DepartmentsLocations;

namespace DS.Domain.Models.Locations;

public class Location
{
    private List<DepartmentLocation> _departmentsLocations = [];

    //ef
    private Location() { }

    private Location(
        Name name,
        Address address,
        Timezone timezone)
    {
        Id = Guid.NewGuid();
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
    public IReadOnlyList<DepartmentLocation> DepartmentsLocations => _departmentsLocations;

    public static Result<Location, string> Create(
        Name name,
        Address adress,
        Timezone timezone)
    {
        return Result.Success<Location, string>(new Location(
            name,
            adress,
            timezone));
    }
}
