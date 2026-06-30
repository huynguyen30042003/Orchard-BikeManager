// DTOs/VehicleOwnerships/UpdateVehicleOwnershipRequest.cs
namespace BikeManagerV3.Customer.DTOs.VehicleOwnerships;

public class UpdateVehicleOwnershipRequest
{
    public Guid SerialNumberId { get; set; }

    public Guid CustomerId { get; set; }

    public Guid OrderId { get; set; }

    public DateTime OwnershipStart { get; set; }

    public DateTime? OwnershipEnd { get; set; }

    public bool IsCurrentOwner { get; set; }
}