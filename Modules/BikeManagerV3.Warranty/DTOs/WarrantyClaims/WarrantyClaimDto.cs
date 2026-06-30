// DTOs/WarrantyClaims/WarrantyClaimDto.cs
namespace BikeManagerV3.Warranty.DTOs.WarrantyClaims;

public class WarrantyClaimDto
{
    public Guid Id { get; set; }

    public Guid WarrantyId { get; set; }

    public Guid RepairOrderId { get; set; }

    public string IssueDescription { get; set; } = string.Empty;

    public string? Resolution { get; set; }

    public string Status { get; set; } = string.Empty;
}