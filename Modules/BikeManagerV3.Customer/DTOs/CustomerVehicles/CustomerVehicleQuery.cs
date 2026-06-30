// DTOs/CustomerVehicles/CustomerVehicleQuery.cs
namespace BikeManagerV3.Customer.DTOs.CustomerVehicles;

public class CustomerVehicleQuery
{
    public Guid? CustomerId { get; set; }

    public string? Search { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}