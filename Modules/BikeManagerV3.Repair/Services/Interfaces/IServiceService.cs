// Services/IServiceService.cs
using BikeManagerV3.Repair.DTOs.Services;
using BikeManagerV3.Repair.Responses;

namespace BikeManagerV3.Repair.Services;

public interface IServiceService
{
    Task<ApiResponse<ServiceResponse>>
        CreateAsync(
            CreateServiceRequest request);

    Task<ApiResponse<List<ServiceResponse>>>
        GetAllAsync(
            ServiceQuery query);

    Task<ApiResponse<ServiceResponse>>
        GetByIdAsync(
            Guid id);

    Task<ApiResponse<ServiceResponse>>
        UpdateAsync(
            Guid id,
            UpdateServiceRequest request);

    Task<ApiResponse<string>>
        DeleteAsync(
            Guid id);
}