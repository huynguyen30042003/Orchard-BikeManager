// DTOs/RepairItems/RepairItemResponse.cs
namespace BikeManagerV3.Repair.DTOs.RepairItems;

public class RepairItemResponse
{
    public Guid Id { get; set; }

    public Guid RepairOrderId { get; set; }

    public string ItemType { get; set; }
        = string.Empty;

    public Guid? ProductVariantId { get; set; }

    public Guid? ServiceId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }
}