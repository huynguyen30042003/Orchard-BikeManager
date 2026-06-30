using BikeManagerV3.Inventory.DTOs;
using BikeManagerV3.Inventory.Responses;

namespace BikeManagerV3.Inventory.Services;

public interface IInventoryStockService
{
    Task<InventoryStockResponse> CreateAsync(
        CreateInventoryStockRequest request);

    Task<List<InventoryStockResponse>> GetAllAsync(
        InventoryStockQuery query);

    Task<PagedResult<InventoryStockResponse>> GetDetailAllAsync(
       InventoryStockQuery query);

    Task<InventoryStockResponse?> GetByIdAsync(
        Guid id);

    Task<InventoryStockResponse?> UpdateAsync(
        Guid id,
        UpdateInventoryStockRequest request);

    Task<bool> DeleteAsync(
        Guid id);
}