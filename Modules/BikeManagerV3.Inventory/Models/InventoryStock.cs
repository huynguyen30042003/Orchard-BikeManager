namespace BikeManagerV3.Inventory.Models;

public class InventoryStock
{
    public Guid Id { get; set; }

    public Guid WarehouseId { get; set; }

    public Guid ProductVariantId { get; set; }

    public int Quantity { get; set; }

    public int ReservedQuantity { get; set; }

    public DateTime UpdatedAt { get; set; }
        = DateTime.UtcNow;
}