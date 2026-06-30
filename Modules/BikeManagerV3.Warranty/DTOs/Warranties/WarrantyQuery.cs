namespace BikeManagerV3.Warranty.DTOs.Warranties;

public class WarrantyQuery
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public Guid? CustomerId { get; set; }

    public Guid? OrderId { get; set; }

    public Guid? SerialNumberId { get; set; }

    public string? Status { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}
