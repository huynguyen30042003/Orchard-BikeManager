// Services/ICustomerVehicleService.cs
using BikeManagerV3.Customer.DTOs.CustomerVehicles;
using BikeManagerV3.Customer.Responses;

namespace BikeManagerV3.Customer.Services;

public interface ICustomerVehicleService
{
    Task<ApiResponse<CustomerVehicleResponse>>
        CreateAsync(
            CreateCustomerVehicleRequest request);
    Task<PagedResult<CustomerVehicleResponse>>
        GetAllAsync(
            CustomerVehicleQuery query);

    Task<ApiResponse<CustomerVehicleResponse>>
        GetByIdAsync(
            Guid id);

    Task<ApiResponse<CustomerVehicleResponse>>
        UpdateAsync(
            Guid id,
            UpdateCustomerVehicleRequest request);

    Task<ApiResponse<string>>
        DeleteAsync(
            Guid id);
}