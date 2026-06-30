namespace BikeManagerV3.Inventory.DTOs.Warehouses;

public class UpdateWarehouseRequest
{
    public string Name { get; set; }
        = string.Empty;

    public string Address { get; set; }
        = string.Empty;
}