namespace BikeManagerV3.Order.DTOs.Sales;

public class CustomerInfoRequest
{
    public string PhoneNumber { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? Address { get; set; }
}