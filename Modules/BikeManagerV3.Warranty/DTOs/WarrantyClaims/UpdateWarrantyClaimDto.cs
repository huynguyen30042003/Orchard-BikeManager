// DTOs/WarrantyClaims/UpdateWarrantyClaimDto.cs
namespace BikeManagerV3.Warranty.DTOs.WarrantyClaims;

public class UpdateWarrantyClaimDto
{
    public string IssueDescription { get; set; } = string.Empty;

    public string? Resolution { get; set; }

    public string Status { get; set; } = "Pending";
}