using BikeManagerV3.Inventory.Data;
using BikeManagerV3.Inventory.DTOs.Warehouses;
using BikeManagerV3.Inventory.Models;
using BikeManagerV3.Inventory.Responses;
using BikeManagerV3.Product.Constants;
using BikeManagerV3.Product.Data;
using BikeManagerV3.Product.Services;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Inventory.Services;

public class WarehouseService : IWarehouseService
{
    private readonly InventoryDbContext _context;
    private readonly CatalogDbContext _contextCatalog;
    private readonly ICounterService _counterService;

    public WarehouseService(
        CatalogDbContext contextCatalog,
        InventoryDbContext context,
        ICounterService counterService)
    {
        _context = context;
        _contextCatalog = contextCatalog;
        _counterService = counterService;
    }

    public async Task<ApiResponse<WarehouseResponse>> CreateAsync(
        CreateWarehouseRequest request)
    {
        ValidateCreateRequest(request);

        var next = await _counterService.GetNextAsync(CounterCodes.Warehouse);

        var Code =
            $"KHO{next:D3}";

        var warehouse = new Warehouse
        {
            Id = Guid.NewGuid(),
            BranchId = request.BranchId,
            Name = request.Name,
            Code = Code,
            Address = request.Address,
            CreatedAt = DateTime.UtcNow
        };

        _context.Warehouses.Add(warehouse);

        await _context.SaveChangesAsync();

        return ApiResponse<WarehouseResponse>.Ok(MapToResponse(warehouse));
    }

    public async Task<PagedResult<WarehouseResponse>> GetAllAsync(
        WarehouseQuery query)
    {
        var warehouses = _context.Warehouses
            .AsQueryable();
        var branchIds = warehouses
             .Select(x => x.BranchId)
             .Distinct()
             .ToList();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            warehouses = warehouses.Where(x =>
                x.Name.Contains(query.Search) ||
                x.Code.Contains(query.Search));
        }

        if (query.BranchId.HasValue)
        {
            warehouses = warehouses.Where(x =>
                x.BranchId == query.BranchId);
        }
        var brands = await _contextCatalog.Brands
            .Where(x => branchIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id);

        var totalItems = await warehouses.CountAsync();

        var items = await warehouses
            .OrderByDescending(x => x.CreatedAt)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new WarehouseResponse
            {
                Id = x.Id,
                BranchId = x.BranchId,

                Branch = brands.ContainsKey(x.BranchId)
                    ? new
                    {
                        brands[x.BranchId].Id,
                        brands[x.BranchId].Name,
                        brands[x.BranchId].Slug,
                        brands[x.BranchId].LogoUrl
                    }
                    : null,

                Name = x.Name,
                Code = x.Code,
                Address = x.Address,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
        return new PagedResult<WarehouseResponse>
        {
            Items = items,
            TotalItems = totalItems,
            Page = query.Page,
            PageSize = query.PageSize,
            TotalPages = (int)Math.Ceiling(
                totalItems / (double)query.PageSize),
        };
    }

    public async Task<ApiResponse<WarehouseResponse>> GetByIdAsync(Guid id)
    {
        var warehouse = await _context.Warehouses
            .FirstOrDefaultAsync(x => x.Id == id);

        if (warehouse == null)
        {
            return null;
        }

        var brand = await _contextCatalog.Brands
            .FirstOrDefaultAsync(x => x.Id == warehouse.BranchId);

        return ApiResponse<WarehouseResponse>.Ok(new WarehouseResponse
        {
            Id = warehouse.Id,

            BranchId = warehouse.BranchId,

            Branch = brand == null
                ? null
                : new
                {
                    brand.Id,
                    brand.Name,
                    brand.Slug,
                    brand.LogoUrl
                },

            Name = warehouse.Name,
            Code = warehouse.Code,
            Address = warehouse.Address,
            CreatedAt = warehouse.CreatedAt
        });
    }

    public async Task<ApiResponse<WarehouseResponse>> UpdateAsync(
        Guid id,
        UpdateWarehouseRequest request)
    {
        ValidateUpdateRequest(request);

        var warehouse = await _context.Warehouses
            .FirstOrDefaultAsync(x => x.Id == id);

        if (warehouse == null)
        {
            return null;
        }

        warehouse.Name = request.Name;
        warehouse.Address = request.Address;

        await _context.SaveChangesAsync();

        return ApiResponse<WarehouseResponse>.Ok(MapToResponse(warehouse));
    }

    public async Task<bool> DeleteAsync(
        Guid id)
    {
        var warehouse = await _context.Warehouses
            .FirstOrDefaultAsync(x => x.Id == id);

        if (warehouse == null)
        {
            return false;
        }

        _context.Warehouses.Remove(warehouse);

        await _context.SaveChangesAsync();

        return true;
    }

    private static WarehouseResponse MapToResponse(
        Warehouse warehouse)
    {
        return new WarehouseResponse
        {
            Id = warehouse.Id,
            BranchId = warehouse.BranchId,
            Name = warehouse.Name,
            Code = warehouse.Code,
            Address = warehouse.Address,
            CreatedAt = warehouse.CreatedAt
        };
    }

    private static void ValidateCreateRequest(
        CreateWarehouseRequest request)
    {
        if (request.BranchId == Guid.Empty)
        {
            throw new Exception("BranchId is required");
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new Exception("Name is required");
        }
    }

    private static void ValidateUpdateRequest(
        UpdateWarehouseRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new Exception("Name is required");
        }
    }
}