// Models/Customer.cs
namespace BikeManagerV3.Customer.Models;

public class CustomerModel
{
    public Guid Id { get; set; }

    public string FullName { get; set; }
        = string.Empty;

    public string PhoneNumber { get; set; }
        = string.Empty;

    public string? Email { get; set; }

    public string? Gender { get; set; }

    public DateTime? Birthday { get; set; }

    public string? Address { get; set; }

    public decimal TotalSpent { get; set; }

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;

    // Navigation
    public CustomerStatistic? Statistic { get; set; }

    public ICollection<CustomerVehicle> Vehicles { get; set; }
        = new List<CustomerVehicle>();

    public ICollection<VehicleOwnership> VehicleOwnerships { get; set; }
        = new List<VehicleOwnership>();
}