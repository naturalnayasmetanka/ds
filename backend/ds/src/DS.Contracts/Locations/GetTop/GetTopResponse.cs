namespace DS.Contracts.Locations.GetTop;

public class GetTopResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public int DepartmentCount { get; set; }
}
