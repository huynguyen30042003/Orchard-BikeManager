namespace BikeManagerV3.Inventory.Models;

public class InventoryCostLayer
{
    public Guid Id { get; set; }

    public Guid ProductVariantId { get; set; }

    public decimal ImportPrice { get; set; }

    public int QuantityRemaining { get; set; }

    public DateTime ImportDate { get; set; }
        = DateTime.UtcNow;
}