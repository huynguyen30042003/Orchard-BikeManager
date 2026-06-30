namespace BikeManagerV3.Inventory.DTOs.Warehouses;

public class WarehouseQuery
{
    public string? Search { get; set; }

    public Guid? BranchId { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}