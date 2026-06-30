// Services/IVehicleOwnershipService.cs
using BikeManagerV3.Customer.DTOs.VehicleOwnerships;
using BikeManagerV3.Customer.Responses;

namespace BikeManagerV3.Customer.Services;

public interface IVehicleOwnershipService
{
    Task<ApiResponse<VehicleOwnershipResponse>>
        CreateAsync(
            CreateVehicleOwnershipRequest request);

    Task<ApiResponse<List<VehicleOwnershipResponse>>>
        GetAllAsync(
            VehicleOwnershipQuery query);

    Task<ApiResponse<VehicleOwnershipResponse>>
        GetByIdAsync(
            Guid id);

    Task<ApiResponse<VehicleOwnershipResponse>>
        UpdateAsync(
            Guid id,
            UpdateVehicleOwnershipRequest request);

    Task<ApiResponse<string>>
        DeleteAsync(
            Guid id);
}