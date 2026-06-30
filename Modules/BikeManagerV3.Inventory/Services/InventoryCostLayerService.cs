using BikeManagerV3.Inventory.Data;
using BikeManagerV3.Inventory.DTOs.InventoryCostLayers;
using BikeManagerV3.Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Inventory.Services;

public class InventoryCostLayerService
    : IInventoryCostLayerService
{
    private readonly InventoryDbContext _context;

    public InventoryCostLayerService(
        InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<InventoryCostLayerResponse> CreateAsync(
        CreateInventoryCostLayerRequest request)
    {
        Validate(request);

        var entity = new InventoryCostLayer
        {
            Id = Guid.NewGuid(),
            ProductVariantId = request.ProductVariantId,
            ImportPrice = request.ImportPrice,
            QuantityRemaining = request.QuantityRemaining,
            ImportDate = request.ImportDate ?? DateTime.UtcNow
        };

        _context.InventoryCostLayers.Add(entity);

        await _context.SaveChangesAsync();

        return Map(entity);
    }

    public async Task<List<InventoryCostLayerResponse>> GetAllAsync(
        InventoryCostLayerQuery query)
    {
        var dbQuery = _context.InventoryCostLayers.AsQueryable();

        if (query.ProductVariantId.HasValue)
        {
            dbQuery = dbQuery.Where(x =>
                x.ProductVariantId == query.ProductVariantId.Value);
        }


        var result = await dbQuery
            .OrderBy(x => x.ImportDate)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return result.Select(Map).ToList();
    }

    public async Task<InventoryCostLayerResponse?> GetByIdAsync(
        Guid id)
    {
        var entity = await _context.InventoryCostLayers
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            return null;
        }

        return Map(entity);
    }

    public async Task<InventoryCostLayerResponse?> UpdateAsync(
        Guid id,
        UpdateInventoryCostLayerRequest request)
    {
        var entity = await _context.InventoryCostLayers
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            return null;
        }

        entity.ImportPrice = request.ImportPrice;
        entity.QuantityRemaining = request.QuantityRemaining;
        entity.ImportDate = request.ImportDate;

        await _context.SaveChangesAsync();

        return Map(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.InventoryCostLayers
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            return false;
        }

        _context.InventoryCostLayers.Remove(entity);

        await _context.SaveChangesAsync();

        return true;
    }

    private static InventoryCostLayerResponse Map(
        InventoryCostLayer entity)
    {
        return new InventoryCostLayerResponse
        {
            Id = entity.Id,
            ProductVariantId = entity.ProductVariantId,
            ImportPrice = entity.ImportPrice,
            QuantityRemaining = entity.QuantityRemaining,
            ImportDate = entity.ImportDate
        };
    }

    private static void Validate(
        CreateInventoryCostLayerRequest request)
    {
        if (request.ProductVariantId == Guid.Empty)
        {
            throw new Exception("ProductVariantId is required");
        }

        if (request.ImportPrice <= 0)
        {
            throw new Exception("ImportPrice must be greater than 0");
        }

        if (request.QuantityRemaining < 0)
        {
            throw new Exception("QuantityRemaining cannot be negative");
        }
    }
}