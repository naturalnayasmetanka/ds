using CSharpFunctionalExtensions;
using DS.Domain.DepartmentsLocations;

namespace DS.Domain.Locations;

public class Location
{
    private List<DepartmentLocation> _departmentsLocations = [];

    private Location(
        Name name,
        Adress adress,
        Timezone timezone)
    {
        Id = Guid.NewGuid();
        Name = name;
        Adress = adress;
        Timezone = timezone;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public Name Name { get; private set; }
    public Adress Adress { get; private set; }
    public Timezone Timezone { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public IReadOnlyList<DepartmentLocation> DepartmentsLocations => _departmentsLocations;

    public static Result<Location, string> Create(
        Name name,
        Adress adress,
        Timezone timezone)
    {
        return Result.Success<Location, string>(new Location(
            name,
            adress,
            timezone));
    }
}
