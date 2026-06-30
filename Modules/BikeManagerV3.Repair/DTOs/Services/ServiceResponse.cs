// DTOs/Services/ServiceResponse.cs
namespace BikeManagerV3.Repair.DTOs.Services;

public class ServiceResponse
{
    public Guid Id { get; set; }

    public string Code { get; set; }
        = string.Empty;

    public string Name { get; set; }
        = string.Empty;

    public string? Description { get; set; }

    public decimal BasePrice { get; set; }

    public int EstimatedMinutes { get; set; }
}