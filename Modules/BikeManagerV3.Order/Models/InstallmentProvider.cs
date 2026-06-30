// Models/InstallmentProvider.cs
namespace BikeManagerV3.Order.Models;

public class InstallmentProvider
{
    public Guid Id { get; set; }

    public string Name { get; set; }
        = string.Empty;

    public string Phone { get; set; }
        = string.Empty;

    public string ApiEndpoint { get; set; }
        = string.Empty;

    public bool IsActive { get; set; }

    // Navigation
    public ICollection<InstallmentContract> Contracts
    { get; set; } = new List<InstallmentContract>();
}