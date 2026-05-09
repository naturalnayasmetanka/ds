  namespace DS.Domain.DepartmentsPositions;

public class DepartmentPosition
{
    private DepartmentPosition() { }

    public Guid Id { get; private set; }
    public Guid DepartmentId { get; private set; }
    public Guid PositionId { get; private set; }

    public DepartmentPosition(
        Guid id,
        Guid departmentId,
        Guid positionId)
    {
        Id = id;
        DepartmentId = departmentId;
        PositionId = positionId;
    }
}
