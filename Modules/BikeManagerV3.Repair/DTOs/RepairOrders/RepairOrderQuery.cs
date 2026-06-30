// DTOs/RepairOrders/RepairOrderQuery.cs
namespace BikeManagerV3.Repair.DTOs.RepairOrders;

public class RepairOrderQuery
{
    public Guid? CustomerId { get; set; }

    public string? Status { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}