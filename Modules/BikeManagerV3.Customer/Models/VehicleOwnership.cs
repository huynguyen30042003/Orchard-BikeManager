// Models/VehicleOwnership.cs
namespace BikeManagerV3.Customer.Models;

public class VehicleOwnership
{
    public Guid Id { get; set; }

    public Guid SerialNumberId { get; set; }

    public Guid CustomerId { get; set; }

    public Guid OrderId { get; set; }

    public DateTime OwnershipStart { get; set; }

    public DateTime? OwnershipEnd { get; set; }

    public bool IsCurrentOwner { get; set; }

    // Navigation
    public CustomerModel Customer { get; set; }
        = null!;
}