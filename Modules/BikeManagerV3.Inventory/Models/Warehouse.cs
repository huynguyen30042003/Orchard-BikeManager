namespace BikeManagerV3.Inventory.Models;

public class Warehouse
{
    public Guid Id { get; set; }

    public Guid BranchId { get; set; }

    public string Name { get; set; } = default!;

    public string Code { get; set; } = default!;

    public string? Address { get; set; }

    public DateTime CreatedAt { get; set; }
}