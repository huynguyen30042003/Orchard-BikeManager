namespace BikeManagerV3.Inventory.DTOs.InventoryCostLayers;

public class InventoryCostLayerQuery
{
    public Guid? ProductVariantId { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}