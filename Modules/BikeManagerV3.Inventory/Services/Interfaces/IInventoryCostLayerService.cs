using BikeManagerV3.Inventory.DTOs.InventoryCostLayers;

namespace BikeManagerV3.Inventory.Services;

public interface IInventoryCostLayerService
{
    Task<InventoryCostLayerResponse> CreateAsync(
        CreateInventoryCostLayerRequest request);

    Task<List<InventoryCostLayerResponse>> GetAllAsync(
        InventoryCostLayerQuery query);

    Task<InventoryCostLayerResponse?> GetByIdAsync(
        Guid id);

    Task<InventoryCostLayerResponse?> UpdateAsync(
        Guid id,
        UpdateInventoryCostLayerRequest request);

    Task<bool> DeleteAsync(
        Guid id);
}