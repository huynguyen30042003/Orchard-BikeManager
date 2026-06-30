// Services/IRepairOrderService.cs
using BikeManagerV3.Repair.DTOs.RepairOrders;
using BikeManagerV3.Repair.Responses;

namespace BikeManagerV3.Repair.Services;

public interface IRepairOrderService
{
    Task<ApiResponse<RepairOrderResponse>>
        CreateAsync(
            CreateRepairOrderRequest request);

    Task<PagedResult<RepairOrderResponse>>
        GetAllAsync(
            RepairOrderQuery query);

    Task<ApiResponse<RepairOrderResponse>>
        GetByIdAsync(
            Guid id);

    Task<ApiResponse<RepairOrderResponse>>
        UpdateAsync(
            Guid id,
            UpdateRepairOrderRequest request);

    Task<ApiResponse<string>>
        DeleteAsync(
            Guid id);
}