using BikeManagerV3.Inventory.Data;
using BikeManagerV3.Inventory.DTOs.InventoryTransactions;
using BikeManagerV3.Inventory.Enums;
using BikeManagerV3.Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Inventory.Services;

public class InventoryTransactionService
    : IInventoryTransactionService
{
    private readonly InventoryDbContext _context;

    public InventoryTransactionService(
        InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<InventoryTransactionResponse> CreateAsync(
        CreateInventoryTransactionRequest request)
    {
        Validate(request);

        var entity = new InventoryTransaction
        {
            Id = Guid.NewGuid(),
            WarehouseId = request.WarehouseId,
            ProductVariantId = request.ProductVariantId,
            TransactionType = request.TransactionType,
            Quantity = request.Quantity,
            BeforeQuantity = request.BeforeQuantity,
            AfterQuantity = request.AfterQuantity,
            ReferenceType = request.ReferenceType,
            ReferenceId = request.ReferenceId,
            Note = request.Note,
            CreatedBy = request.CreatedBy,
            CreatedAt = DateTime.UtcNow
        };

        _context.InventoryTransactions.Add(entity);

        await _context.SaveChangesAsync();

        return Map(entity);
    }

    public async Task<List<InventoryTransactionResponse>> GetAllAsync(
        InventoryTransactionQuery query)
    {
        var dbQuery = _context.InventoryTransactions.AsQueryable();

        if (query.WarehouseId.HasValue)
        {
            dbQuery = dbQuery.Where(x =>
                x.WarehouseId == query.WarehouseId.Value);
        }

        if (query.ProductVariantId.HasValue)
        {
            dbQuery = dbQuery.Where(x =>
                x.ProductVariantId == query.ProductVariantId.Value);
        }

        if (query.TransactionType.HasValue)
        {
            dbQuery = dbQuery.Where(x =>
                x.TransactionType == query.TransactionType);
        }

        var result = await dbQuery
            .OrderByDescending(x => x.CreatedAt)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return result.Select(Map).ToList();
    }

    public async Task<InventoryTransactionResponse?> GetByIdAsync(
        Guid id)
    {
        var entity = await _context.InventoryTransactions
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            return null;
        }

        return Map(entity);
    }

    public async Task<InventoryTransactionResponse?> UpdateAsync(
        Guid id,
        UpdateInventoryTransactionRequest request)
    {
        var entity = await _context.InventoryTransactions
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            return null;
        }

        entity.TransactionType = request.TransactionType;
        entity.Quantity = request.Quantity;
        entity.BeforeQuantity = request.BeforeQuantity;
        entity.AfterQuantity = request.AfterQuantity;
        entity.ReferenceType = request.ReferenceType;
        entity.ReferenceId = request.ReferenceId;
        entity.Note = request.Note;

        await _context.SaveChangesAsync();

        return Map(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.InventoryTransactions
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            return false;
        }

        _context.InventoryTransactions.Remove(entity);

        await _context.SaveChangesAsync();

        return true;
    }

    private static InventoryTransactionResponse Map(
        InventoryTransaction entity)
    {
        return new InventoryTransactionResponse
        {
            Id = entity.Id,
            WarehouseId = entity.WarehouseId,
            ProductVariantId = entity.ProductVariantId,
            TransactionType = entity.TransactionType,
            Quantity = entity.Quantity,
            BeforeQuantity = entity.BeforeQuantity,
            AfterQuantity = entity.AfterQuantity,
            ReferenceType = entity.ReferenceType,
            ReferenceId = entity.ReferenceId,
            Note = entity.Note,
            CreatedBy = entity.CreatedBy,
            CreatedAt = entity.CreatedAt
        };
    }

    private static void Validate(
        CreateInventoryTransactionRequest request)
    {
        if (request.WarehouseId == Guid.Empty)
        {
            throw new Exception("WarehouseId is required");
        }

        if (request.ProductVariantId == Guid.Empty)
        {
            throw new Exception("ProductVariantId is required");
        }

        if (!Enum.IsDefined(typeof(InventoryTransactionType),
    request.TransactionType))
        {
            throw new Exception("TransactionType is required");
        }

        if (request.Quantity <= 0)
        {
            throw new Exception("Quantity must be greater than 0");
        }
    }
}