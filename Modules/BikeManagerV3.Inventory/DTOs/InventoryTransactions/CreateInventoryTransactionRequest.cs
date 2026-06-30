// DTOs/InventoryTransactions/CreateInventoryTransactionRequest.cs
using BikeManagerV3.Inventory.Enums;

namespace BikeManagerV3.Inventory.DTOs.InventoryTransactions;

public class CreateInventoryTransactionRequest
{
    public Guid WarehouseId { get; set; }

    public Guid ProductVariantId { get; set; }

    public InventoryTransactionType TransactionType { get; set; }

    public int Quantity { get; set; }

    public int BeforeQuantity { get; set; }

    public int AfterQuantity { get; set; }

    public string? ReferenceType { get; set; }

    public Guid? ReferenceId { get; set; }

    public string? Note { get; set; }

    public string CreatedBy { get; set; }
}