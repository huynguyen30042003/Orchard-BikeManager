using System.ComponentModel.DataAnnotations;

namespace BikeManagerV3.Warranty.Models;

public class WarrantyModel
{
    public Guid Id { get; set; }

    public Guid SerialNumberId { get; set; }

    public Guid CustomerId { get; set; }

    public Guid OrderId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "Active";

    public ICollection<WarrantyClaim> WarrantyClaims { get; set; }
        = new List<WarrantyClaim>();
}