// Models/RepairOrder.cs
namespace BikeManagerV3.Repair.Models;

public class RepairOrder
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public Guid CustomerVehicleId { get; set; }

    public string RepairCode { get; set; }
        = string.Empty;

    public string? IssueDescription { get; set; }

    public string? Diagnosis { get; set; }

    public string Status { get; set; }
        = "repairing";

    public decimal EstimatedCost { get; set; }

    public decimal TotalCost { get; set; }

    public DateTime CheckInAt { get; set; }

    public DateTime? CompletedAt { get; set; }
}