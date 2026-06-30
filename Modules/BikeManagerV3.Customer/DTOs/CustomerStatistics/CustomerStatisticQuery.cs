// DTOs/CustomerStatistics/CustomerStatisticQuery.cs
namespace BikeManagerV3.Customer.DTOs.CustomerStatistics;

public class CustomerStatisticQuery
{
    public string? CustomerLevel { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}