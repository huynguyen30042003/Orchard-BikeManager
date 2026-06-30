namespace BikeManagerV3.Inventory.DTOs;

public class CreateInventoryStockRequest
{
    public Guid WarehouseId { get; set; }

    public Guid ProductVariantId { get; set; }

    public int Quantity { get; set; }

    public int ReservedQuantity { get; set; }
}