// DTOs/RepairItems/UpdateRepairItemRequest.cs
namespace BikeManagerV3.Repair.DTOs.RepairItems;

public class UpdateRepairItemRequest
{
    public string ItemType { get; set; }
        = string.Empty;

    public Guid? ProductVariantId { get; set; }

    public Guid? ServiceId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }
}