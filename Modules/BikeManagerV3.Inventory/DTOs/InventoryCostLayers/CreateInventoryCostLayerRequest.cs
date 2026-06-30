namespace BikeManagerV3.Inventory.DTOs.InventoryCostLayers;

public class CreateInventoryCostLayerRequest
{
    public Guid ProductVariantId { get; set; }

    public decimal ImportPrice { get; set; }

    public int QuantityRemaining { get; set; }

    public DateTime? ImportDate { get; set; }
}