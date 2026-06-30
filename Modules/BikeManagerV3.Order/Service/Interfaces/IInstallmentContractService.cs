// Services/IInstallmentContractService.cs
using BikeManagerV3.Order.DTOs.InstallmentContracts;
using BikeManagerV3.Order.Responses;

namespace BikeManagerV3.Order.Services;

public interface IInstallmentContractService
{
    Task<ApiResponse<
        InstallmentContractResponse>>
        CreateAsync(
            CreateInstallmentContractRequest request);

    Task<PagedResult<InstallmentContractResponse>>
        GetAllAsync(
            InstallmentContractQuery query);

    Task<ApiResponse<
        InstallmentContractResponse>>
        GetByIdAsync(
            Guid id);
    Task<ApiResponse<
        InstallmentContractResponse>>
        GetByOrderIdAsync(
            Guid orderId);

    Task<ApiResponse<
        InstallmentContractResponse>>
        UpdateAsync(
            Guid id,
            UpdateInstallmentContractRequest request);

    Task<ApiResponse<string>>
        DeleteAsync(
            Guid id);
}