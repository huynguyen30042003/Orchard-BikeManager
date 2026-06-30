// DTOs/InstallmentProviders/UpdateInstallmentProviderRequest.cs
namespace BikeManagerV3.Order.DTOs.InstallmentProviders;

public class UpdateInstallmentProviderRequest
{
    public string Name { get; set; }
        = string.Empty;

    public string Phone { get; set; }
        = string.Empty;

    public string ApiEndpoint { get; set; }
        = string.Empty;

    public bool IsActive { get; set; }
}