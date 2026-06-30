// DTOs/InstallmentProviders/InstallmentProviderQuery.cs
namespace BikeManagerV3.Order.DTOs.InstallmentProviders;

public class InstallmentProviderQuery
{
    public string? Search { get; set; }

    public bool? IsActive { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}