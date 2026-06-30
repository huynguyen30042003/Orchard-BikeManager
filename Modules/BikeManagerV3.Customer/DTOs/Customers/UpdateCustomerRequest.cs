namespace BikeManagerV3.Customer.DTOs.Customers;

public class UpdateCustomerRequest
{
    public string FullName { get; set; }
        = string.Empty;

    public string PhoneNumber { get; set; }
        = string.Empty;

    public string? Email { get; set; }

    public string? Gender { get; set; }

    public DateTime? Birthday { get; set; }

    public string? Address { get; set; }

    public decimal TotalSpent { get; set; }
}
