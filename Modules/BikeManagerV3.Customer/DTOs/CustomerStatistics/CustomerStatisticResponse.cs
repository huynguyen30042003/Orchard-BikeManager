// DTOs/CustomerStatistics/CustomerStatisticResponse.cs
using BikeManagerV3.Customer.DTOs.Customers;
using BikeManagerV3.Customer.Models;

namespace BikeManagerV3.Customer.DTOs.CustomerStatistics;

public class CustomerStatisticResponse
{
    public Guid CustomerId { get; set; }

    public int TotalOrders { get; set; }

    public decimal TotalSpent { get; set; }

    public int TotalRepairs { get; set; }

    public DateTime? LastPurchaseAt { get; set; }

    public string CustomerLevel { get; set; }
        = string.Empty;

    public decimal DiscountRate { get; set; }

    public CustomerResponse? Customer { get; set; }
}