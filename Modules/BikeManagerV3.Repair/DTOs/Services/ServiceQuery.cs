// DTOs/Services/ServiceQuery.cs
namespace BikeManagerV3.Repair.DTOs.Services;

public class ServiceQuery
{
    public string? Search { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}