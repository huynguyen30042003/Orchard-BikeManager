namespace BikeManagerV3.Customer.DTOs.CustomerStatistics;

public class UpdateCustomerStatisticRequest
{
    public int TotalOrders { get; set; }

    public decimal TotalSpent { get; set; }

    public int TotalRepairs { get; set; }

    public DateTime? LastPurchaseAt { get; set; }

    public string CustomerLevel { get; set; }
        = string.Empty;

    public decimal DiscountRate { get; set; }
}