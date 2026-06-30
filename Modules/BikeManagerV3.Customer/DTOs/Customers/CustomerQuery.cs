namespace BikeManagerV3.Customer.DTOs.Customers;

public class CustomerQuery
{
    public string? Search { get; set; }

    public string? SearchBy { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}