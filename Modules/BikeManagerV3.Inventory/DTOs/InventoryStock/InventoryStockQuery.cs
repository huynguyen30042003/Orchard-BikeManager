namespace BikeManagerV3.Inventory.DTOs;

public class InventoryStockQuery
{
    public Guid? WarehouseId { get; set; }

    public Guid? ProductVariantId { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}