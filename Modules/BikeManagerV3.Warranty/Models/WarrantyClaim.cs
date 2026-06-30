// Models/WarrantyClaim.cs
using System.ComponentModel.DataAnnotations;

namespace BikeManagerV3.Warranty.Models;

public class WarrantyClaim
{
    public Guid Id { get; set; }

    public Guid WarrantyId { get; set; }

    public Guid RepairOrderId { get; set; }

    public string IssueDescription { get; set; } = string.Empty;

    public string? Resolution { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "Pending";

    public WarrantyModel? Warranty { get; set; }
}