namespace DS.Contracts.Locations.Get;

public record GetLocationsResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public AddressDto Address { get; set; } = default!;
    public string Timezone { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
