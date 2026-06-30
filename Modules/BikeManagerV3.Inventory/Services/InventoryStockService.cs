using BikeManagerV3.Inventory.Data;
using BikeManagerV3.Inventory.DTOs;
using BikeManagerV3.Inventory.Models;
using BikeManagerV3.Inventory.Responses;
using BikeManagerV3.Product.Data;
using BikeManagerV3.Product.DTOs.Brand;
using BikeManagerV3.Product.DTOs.Category;
using BikeManagerV3.Product.DTOs.ProductVariant;
using BikeManagerV3.Product.Product.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Inventory.Services;

public class InventoryStockService
    : IInventoryStockService
{
    private readonly InventoryDbContext _context;
    private readonly CatalogDbContext _catalogDB;


    public InventoryStockService(
        InventoryDbContext context, CatalogDbContext catalogDB)
    {
        _context = context;
        _catalogDB = catalogDB;
    }

    public async Task<InventoryStockResponse> CreateAsync(
        CreateInventoryStockRequest request)
    {
        ValidateCreate(request);

        var existed = await _context.InventoryStocks
            .AnyAsync(x =>
                x.WarehouseId == request.WarehouseId &&
                x.ProductVariantId == request.ProductVariantId);

        if (existed)
        {
            throw new Exception(
                "Inventory stock already exists");
        }

        var stock = new InventoryStock
        {
            Id = Guid.NewGuid(),
            WarehouseId = request.WarehouseId,
            ProductVariantId = request.ProductVariantId,
            Quantity = request.Quantity,
            ReservedQuantity = request.ReservedQuantity,
            UpdatedAt = DateTime.UtcNow
        };

        _context.InventoryStocks.Add(stock);
        await _context.SaveChangesAsync();

        return Map(stock);
    }

    public async Task<List<InventoryStockResponse>>
        GetAllAsync(InventoryStockQuery query)
    {
        var stocks = _context.InventoryStocks
            .AsQueryable();

        if (query.WarehouseId.HasValue)
        {
            stocks = stocks.Where(x =>
                x.WarehouseId == query.WarehouseId.Value);
        }

        if (query.ProductVariantId.HasValue)
        {
            stocks = stocks.Where(x =>
                x.ProductVariantId ==
                query.ProductVariantId.Value);
        }

        return await stocks
            .OrderByDescending(x => x.UpdatedAt)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new InventoryStockResponse
            {
                Id = x.Id,
                WarehouseId = x.WarehouseId,
                ProductVariantId = x.ProductVariantId,
                Quantity = x.Quantity,
                ReservedQuantity = x.ReservedQuantity,
                UpdatedAt = x.UpdatedAt
            })
            .ToListAsync();
    }

    public async Task<PagedResult<InventoryStockResponse>>
       GetDetailAllAsync(InventoryStockQuery query)
    {
        var stocks = _context.InventoryStocks
            .AsQueryable();

        if (query.WarehouseId.HasValue)
        {
            stocks = stocks.Where(x =>
                x.WarehouseId == query.WarehouseId.Value);
        }

        if (query.ProductVariantId.HasValue)
        {
            stocks = stocks.Where(x =>
                x.ProductVariantId ==
                query.ProductVariantId.Value);
        }

        var stockList = await stocks
            .OrderByDescending(x => x.UpdatedAt)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        var variantIds = stockList
            .Select(x => x.ProductVariantId)
            .Distinct()
            .ToList();
        var variants = await _catalogDB.ProductVariants
            .Include(x => x.Product)
                .ThenInclude(x => x.Category)
                    .ThenInclude(x => x.Parent)
            .Include(x => x.Product)
                .ThenInclude(x => x.Brand)
            .Where(x => variantIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id);


        var result = stockList
            .Where(stock => variants.ContainsKey(stock.ProductVariantId))
            .Select(stock =>
            {
                var variant = variants[stock.ProductVariantId];

                return new InventoryStockResponse
                {
                    Id = stock.Id,
                    WarehouseId = stock.WarehouseId,
                    ProductVariantId = stock.ProductVariantId,
                    Quantity = stock.Quantity,
                    ReservedQuantity = stock.ReservedQuantity,
                    UpdatedAt = stock.UpdatedAt,

                    ProductVariant = new ProductVariantResponse
                    {
                        Id = variant.Id,
                        ProductId = variant.ProductId,
                        SKU = variant.SKU,
                        Color = variant.Color,
                        Battery = variant.Battery,
                        MotorPower = variant.MotorPower,
                        ImportPrice = variant.ImportPrice,
                        SellingPrice = variant.SellingPrice,
                        WholesalePrice = variant.WholesalePrice,
                        StockQuantity = variant.StockQuantity,
                        WarrantyMonths = variant.WarrantyMonths,
                        TrackSerial = variant.TrackSerial,
                        Product = new ProductSimpleDto
                        {
                            Id = variant.Product.Id,
                            CategoryId = variant.Product.CategoryId,
                            BrandId = variant.Product.BrandId,
                            Barcode = variant.Product.Barcode,
                            Name = variant.Product.Name,
                            Slug = variant.Product.Slug,
                            ShortDescription = variant.Product.ShortDescription,
                            Description = variant.Product.Description,
                            ThumbnailUrl = variant.Product.ThumbnailUrl,
                            ProductType = variant.Product.ProductType,

                            Category = new CategoryDto
                            {
                                Id = variant.Product.Category.Id,
                                ParentId = variant.Product.Category.ParentId,
                                Name = variant.Product.Category.Name,
                                Slug = variant.Product.Category.Slug,
                                ImageUrl = variant.Product.Category.ImageUrl,
                                Description = variant.Product.Category.Description,
                                IsActive = variant.Product.Category.IsActive,
                                SortOrder = variant.Product.Category.SortOrder,

                                Parent = variant.Product.Category.Parent == null
                                    ? null
                                    : new CategoryParentDto
                                    {
                                        Id = variant.Product.Category.Parent.Id,
                                        Name = variant.Product.Category.Parent.Name,
                                        Slug = variant.Product.Category.Parent.Slug
                                    }
                            },

                            Brand = new BrandDto
                            {
                                Id = variant.Product.Brand.Id,
                                Name = variant.Product.Brand.Name,
                                LogoUrl = variant.Product.Brand.LogoUrl
                            }
                        }
                    }
                };
            })
            .ToList();
        var totalItems = await stocks.CountAsync();
        return new PagedResult<InventoryStockResponse>
        {
            Page = query.Page,
            PageSize = query.PageSize,
            TotalItems = totalItems,

            TotalPages = (int)Math.Ceiling(
                totalItems / (double)query.PageSize),

            Items = result
        };
    }

    public async Task<InventoryStockResponse?>
        GetByIdAsync(Guid id)
    {
        var stock = await _context.InventoryStocks
            .FirstOrDefaultAsync(x => x.Id == id);

        if (stock == null)
        {
            return null;
        }

        return Map(stock);
    }

    public async Task<InventoryStockResponse?>
        UpdateAsync(
            Guid id,
            UpdateInventoryStockRequest request)
    {
        ValidateUpdate(request);

        var stock = await _context.InventoryStocks
            .FirstOrDefaultAsync(x => x.Id == id);

        if (stock == null)
        {
            return null;
        }

        stock.Quantity = request.Quantity;
        stock.ReservedQuantity =
            request.ReservedQuantity;

        stock.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Map(stock);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var stock = await _context.InventoryStocks
            .FirstOrDefaultAsync(x => x.Id == id);

        if (stock == null)
        {
            return false;
        }

        _context.InventoryStocks.Remove(stock);

        await _context.SaveChangesAsync();

        return true;
    }

    private static InventoryStockResponse Map(
        InventoryStock stock)
    {
        return new InventoryStockResponse
        {
            Id = stock.Id,
            WarehouseId = stock.WarehouseId,
            ProductVariantId = stock.ProductVariantId,
            Quantity = stock.Quantity,
            ReservedQuantity = stock.ReservedQuantity,
            UpdatedAt = stock.UpdatedAt
        };
    }

    private static void ValidateCreate(
        CreateInventoryStockRequest request)
    {
        if (request.WarehouseId == Guid.Empty)
        {
            throw new Exception(
                "WarehouseId is required");
        }

        if (request.ProductVariantId == Guid.Empty)
        {
            throw new Exception(
                "ProductVariantId is required");
        }

        if (request.Quantity < 0)
        {
            throw new Exception(
                "Quantity cannot be negative");
        }

        if (request.ReservedQuantity < 0)
        {
            throw new Exception(
                "ReservedQuantity cannot be negative");
        }
    }

    private static void ValidateUpdate(
        UpdateInventoryStockRequest request)
    {
        if (request.Quantity < 0)
        {
            throw new Exception(
                "Quantity cannot be negative");
        }

        if (request.ReservedQuantity < 0)
        {
            throw new Exception(
                "ReservedQuantity cannot be negative");
        }
    }
}