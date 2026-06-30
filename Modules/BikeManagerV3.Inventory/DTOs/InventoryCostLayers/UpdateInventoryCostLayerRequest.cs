namespace BikeManagerV3.Inventory.DTOs.InventoryCostLayers;

public class UpdateInventoryCostLayerRequest
{
    public decimal ImportPrice { get; set; }

    public int QuantityRemaining { get; set; }

    public DateTime ImportDate { get; set; }
}