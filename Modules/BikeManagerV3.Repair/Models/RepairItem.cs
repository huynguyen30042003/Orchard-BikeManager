// Models/RepairItem.cs
namespace BikeManagerV3.Repair.Models;

public class RepairItem
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