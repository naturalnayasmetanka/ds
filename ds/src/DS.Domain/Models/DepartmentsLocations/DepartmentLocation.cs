namespace DS.Domain.Models.DepartmentsLocations;

public class DepartmentLocation
{
    //ef
    private DepartmentLocation() { }

    public Guid DepartmentId { get; private set; }
    public Guid LocationId { get; private set; }

    public DepartmentLocation(
       Guid departmentId,
       Guid locationId)
    {
        DepartmentId = departmentId;
        LocationId = locationId;
    }
}
