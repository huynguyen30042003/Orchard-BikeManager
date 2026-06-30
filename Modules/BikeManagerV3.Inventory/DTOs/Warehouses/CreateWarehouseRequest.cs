namespace BikeManagerV3.Inventory.DTOs.Warehouses;

public class CreateWarehouseRequest
{
    public Guid BranchId { get; set; }

    public string Name { get; set; }
        = string.Empty;

    public string Address { get; set; }
        = string.Empty;
}