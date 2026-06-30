using BikeManagerV3.Inventory.DTOs.InventoryTransactions;

namespace BikeManagerV3.Inventory.Services;

public interface IInventoryTransactionService
{
    Task<InventoryTransactionResponse> CreateAsync(
        CreateInventoryTransactionRequest request);

    Task<InventoryTransactionResponse?> GetByIdAsync(
        Guid id);

    Task<List<InventoryTransactionResponse>> GetAllAsync(
        InventoryTransactionQuery query);

    Task<InventoryTransactionResponse?> UpdateAsync(
        Guid id,
        UpdateInventoryTransactionRequest request);

    Task<bool> DeleteAsync(
        Guid id);
}