// Services/IInstallmentProviderService.cs
using BikeManagerV3.Order.DTOs.InstallmentProviders;
using BikeManagerV3.Order.Responses;

namespace BikeManagerV3.Order.Services;

public interface IInstallmentProviderService
{
    Task<ApiResponse<InstallmentProviderResponse>>
        CreateAsync(
            CreateInstallmentProviderRequest request);

    Task<PagedResult<InstallmentProviderResponse>>
        GetAllAsync(
            InstallmentProviderQuery query);

    Task<ApiResponse<
        InstallmentProviderResponse>>
        GetByIdAsync(
            Guid id);

    Task<ApiResponse<
        InstallmentProviderResponse>>
        UpdateAsync(
            Guid id,
            UpdateInstallmentProviderRequest request);

    Task<ApiResponse<string>>
        DeleteAsync(
            Guid id);
}