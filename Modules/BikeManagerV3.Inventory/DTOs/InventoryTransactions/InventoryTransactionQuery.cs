using BikeManagerV3.Inventory.Enums;

namespace BikeManagerV3.Inventory.DTOs.InventoryTransactions;

public class InventoryTransactionQuery
{
    public Guid? WarehouseId { get; set; }

    public Guid? ProductVariantId { get; set; }

    public InventoryTransactionType? TransactionType { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}