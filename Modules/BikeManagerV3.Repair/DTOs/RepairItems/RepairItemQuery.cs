// DTOs/RepairItems/RepairItemQuery.cs
namespace BikeManagerV3.Repair.DTOs.RepairItems;

public class RepairItemQuery
{
    public Guid? RepairOrderId { get; set; }

    public string? ItemType { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}