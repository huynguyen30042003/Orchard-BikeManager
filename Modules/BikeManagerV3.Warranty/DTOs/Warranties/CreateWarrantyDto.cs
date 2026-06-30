// DTOs/Warranties/CreateWarrantyDto.cs
namespace BikeManagerV3.Warranty.DTOs.Warranties;

public class CreateWarrantyDto
{
    public Guid SerialNumberId { get; set; }

    public Guid CustomerId { get; set; }

    public Guid OrderId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public string Status { get; set; } = "Active";
}