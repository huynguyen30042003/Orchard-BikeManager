// DTOs/RepairOrders/UpdateRepairOrderRequest.cs
namespace BikeManagerV3.Repair.DTOs.RepairOrders;

public class UpdateRepairOrderRequest
{
    public string RepairCode { get; set; }
        = string.Empty;

    public string? IssueDescription { get; set; }

    public string? Diagnosis { get; set; }

    public string Status { get; set; }
        = string.Empty;

    public decimal EstimatedCost { get; set; }

    public decimal TotalCost { get; set; }

    public DateTime CheckInAt { get; set; }

    public DateTime? CompletedAt { get; set; }
}