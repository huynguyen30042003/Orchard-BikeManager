// Services/IRepairItemService.cs
using BikeManagerV3.Repair.DTOs.RepairItems;
using BikeManagerV3.Repair.Responses;

namespace BikeManagerV3.Repair.Services;

public interface IRepairItemService
{
    Task<ApiResponse<RepairItemResponse>>
        CreateAsync(
            CreateRepairItemRequest request);

    Task<ApiResponse<List<RepairItemResponse>>>
        GetAllAsync(
            RepairItemQuery query);

    Task<ApiResponse<RepairItemResponse>>
        GetByIdAsync(
            Guid id);

    Task<ApiResponse<RepairItemResponse>>
        UpdateAsync(
            Guid id,
            UpdateRepairItemRequest request);

    Task<ApiResponse<string>>
        DeleteAsync(
            Guid id);
}