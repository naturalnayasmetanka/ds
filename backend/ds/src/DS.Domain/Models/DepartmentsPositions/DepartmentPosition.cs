namespace DS.Domain.Models.DepartmentsPositions;

public class DepartmentPosition
{
    //ef
    private DepartmentPosition() { }

    public Guid DepartmentId { get; private set; }
    public Guid PositionId { get; private set; }

    public DepartmentPosition(
        Guid departmentId,
        Guid positionId)
    {
        DepartmentId = departmentId;
        PositionId = positionId;
    }
}
