// Models/CustomerStatistic.cs
namespace BikeManagerV3.Customer.Models;

public class CustomerStatistic
{
    public Guid CustomerId { get; set; }

    public int TotalOrders { get; set; }

    public decimal TotalSpent { get; set; }

    public int TotalRepairs { get; set; }

    public DateTime? LastPurchaseAt { get; set; }

    public string CustomerLevel { get; set; }
        = "Normal";

    public decimal DiscountRate { get; set; }

    // Navigation
    public CustomerModel Customer { get; set; }
        = null!;
}