namespace DS.Domain.DepartmentsLocations;

public class DepartmentLocation
{
    private DepartmentLocation() { }

    public Guid Id { get; private set; }
    public Guid DepartmentId { get; private set; }
    public Guid LocationId { get; private set; }

    public DepartmentLocation(
       Guid id,
       Guid departmentId,
       Guid locationId)
    {
        Id = id;
        DepartmentId = departmentId;
        LocationId = locationId;
    }
}
