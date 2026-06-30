using BikeManagerV3.Inventory.DTOs.Warehouses;
using BikeManagerV3.Inventory.Responses;

namespace BikeManagerV3.Inventory.Services;

public interface IWarehouseService
{
    Task<ApiResponse<WarehouseResponse>> CreateAsync(
        CreateWarehouseRequest request);

    Task<ApiResponse<WarehouseResponse>> GetByIdAsync(
        Guid id);

    Task<PagedResult<WarehouseResponse>> GetAllAsync(
        WarehouseQuery query);

    Task<ApiResponse<WarehouseResponse>> UpdateAsync(
        Guid id,
        UpdateWarehouseRequest request);

    Task<bool> DeleteAsync(
        Guid id);
}