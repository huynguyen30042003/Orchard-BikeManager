using BikeManagerV3.Product.DTOs.ProductVariant;

namespace BikeManagerV3.Inventory.DTOs;

public class InventoryStockResponse
{
    public Guid Id { get; set; }

    public Guid WarehouseId { get; set; }

    public Guid ProductVariantId { get; set; }

    public int Quantity { get; set; }

    public int ReservedQuantity { get; set; }

    public DateTime UpdatedAt { get; set; }

    public ProductVariantResponse ProductVariant { get; set; }
}