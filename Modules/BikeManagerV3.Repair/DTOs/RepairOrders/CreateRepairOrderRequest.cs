// DTOs/RepairOrders/CreateRepairOrderRequest.cs
namespace BikeManagerV3.Repair.DTOs.RepairOrders;

public class CreateRepairOrderRequest
{
    public Guid CustomerId { get; set; }

    public Guid CustomerVehicleId { get; set; }

    public string? IssueDescription { get; set; }

    public string? Diagnosis { get; set; }

    public string Status { get; set; }
        = "repairing";

    public decimal EstimatedCost { get; set; }

    public decimal TotalCost { get; set; }

    public DateTime CheckInAt { get; set; }

    public DateTime? CompletedAt { get; set; }
}