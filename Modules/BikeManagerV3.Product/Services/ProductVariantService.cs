using BikeManagerV3.Product.Constants;
using BikeManagerV3.Product.Data;
using BikeManagerV3.Product.DTOs.Brand;
using BikeManagerV3.Product.DTOs.Category;
using BikeManagerV3.Product.DTOs.ProductVariant;
using BikeManagerV3.Product.Enums;
using BikeManagerV3.Product.Models;
using BikeManagerV3.Product.Product.DTOs;
using BikeManagerV3.Product.Responses;
using BikeManagerV3.Product.Services.Interfaces;
using BikeManagerV3.Product.Validators;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Product.Services;

public class ProductVariantService
    : IProductVariantService
{
    private readonly CatalogDbContext _context;
    private readonly ICounterService _counterService;

    public ProductVariantService(
        CatalogDbContext context, ICounterService counterService)
    {
        _context = context;
        _counterService = counterService;
    }

    public async Task<PagedResult<ProductVariantResponse>> GetAll(
        ProductVariantQuery query)
    {
        const string Collation = "Vietnamese_100_CI_AI";
        var variants = _context.ProductVariants
            .Include(x => x.Product.Brand)
            .Include(x => x.Product)
            .AsQueryable();
        if (query.TrackSerial.HasValue)
        {
            variants = variants.Where(x =>
                x.TrackSerial ==
                query.TrackSerial.Value);
        }
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var keyword = query.Search.Trim();

            switch (query.SearchBy?.ToLower())
            {
                case "sku":
                    variants = variants.Where(x =>
                        EF.Functions.Collate(
                            x.SKU,
                            Collation
                        ).Contains(keyword));

                    break;

                case "brand":
                    variants = variants.Where(x =>
                        EF.Functions.Collate(
                            x.Product.Brand.Name,
                            Collation
                        ).Contains(keyword));

                    break;

                case "name":
                    variants = variants.Where(x =>
                        x.Product.Name != null &&

                        EF.Functions.Collate(
                            x.Product.Name,
                            Collation
                        ).Contains(keyword));

                    break;

                case "color":
                    variants = variants.Where(x =>
                        x.Color != null &&

                        EF.Functions.Collate(
                            x.Color,
                            Collation
                        ).Contains(keyword));

                    break;

                default:
                    variants = variants.Where(x =>

                        EF.Functions.Collate(
                            x.SKU,
                            Collation
                        ).Contains(keyword)

                        ||

                        EF.Functions.Collate(
                            x.Product.Brand.Name,
                            Collation
                        ).Contains(keyword)

                        ||

                        (x.Product.Name != null &&

                         EF.Functions.Collate(
                             x.Product.Name,
                             Collation
                         ).Contains(keyword))

                        ||

                        (x.Color != null &&

                         EF.Functions.Collate(
                             x.Color,
                             Collation
                         ).Contains(keyword))
                    );

                    break;
            }
        }

        if (query.MinPrice.HasValue)
        {
            variants = variants.Where(x =>
                x.SellingPrice >=
                query.MinPrice.Value);
        }

        if (query.MaxPrice.HasValue)
        {
            variants = variants.Where(x =>
                x.SellingPrice <=
                query.MaxPrice.Value);
        }
        var totalItems = await variants.CountAsync();

        var result = await variants
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new ProductVariantResponse
            {
                Id = x.Id,
                ProductId = x.ProductId,
                SKU = x.SKU,
                Color = x.Color,
                Battery = x.Battery,
                MotorPower = x.MotorPower,
                ImportPrice = x.ImportPrice,
                SellingPrice = x.SellingPrice,
                WholesalePrice = x.WholesalePrice,
                StockQuantity = x.StockQuantity,
                WarrantyMonths = x.WarrantyMonths,
                TrackSerial = x.TrackSerial,
                Product = new ProductSimpleDto
                {
                    Id = x.Product.Id,
                    CategoryId = x.Product.CategoryId,
                    BrandId = x.Product.BrandId,

                    Barcode = x.Product.Barcode,

                    Name = x.Product.Name,
                    Slug = x.Product.Slug,

                    ShortDescription = x.Product.ShortDescription,
                    Description = x.Product.Description,

                    ThumbnailUrl = x.Product.ThumbnailUrl,

                    ProductType = x.Product.ProductType,

                    //    //Category DTO
                    Category = new CategoryDto
                    {
                        Id = x.Product.Category.Id,
                        ParentId = x.Product.Category.ParentId,

                        Name = x.Product.Category.Name,
                        Slug = x.Product.Category.Slug,

                        ImageUrl = x.Product.Category.ImageUrl,
                        Description = x.Product.Category.Description,

                        IsActive = x.Product.Category.IsActive,
                        SortOrder = x.Product.Category.SortOrder,

                        // Parent Category DTO
                        Parent = x.Product.Category.Parent == null
                                ? null
                                : new CategoryParentDto
                                {
                                    Id = x.Product.Category.Parent.Id,
                                    Name = x.Product.Category.Parent.Name,
                                    Slug = x.Product.Category.Parent.Slug
                                }
                    },

                    // Brand DTO
                    Brand = new BrandDto
                    {
                        Id = x.Product.Brand.Id,
                        Name = x.Product.Brand.Name,
                        LogoUrl = x.Product.Brand.LogoUrl
                    }
                }
            })
            .ToListAsync();

        return new PagedResult<ProductVariantResponse>
        {
            Page = query.Page,
            PageSize = query.PageSize,
            TotalItems = totalItems,

            TotalPages = (int)Math.Ceiling(
                totalItems / (double)query.PageSize),

            Items = result
        };
    }

    public async Task<ApiResponse<object>> GetById(Guid id)
    {
        var variant = await _context.ProductVariants
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new ProductVariantResponse
            {
                Id = x.Id,
                ProductId = x.ProductId,
                SKU = x.SKU,
                Color = x.Color,
                Battery = x.Battery,
                MotorPower = x.MotorPower,

                ImportPrice = x.ImportPrice,
                SellingPrice = x.SellingPrice,
                WholesalePrice = x.WholesalePrice,

                StockQuantity = x.StockQuantity,
                WarrantyMonths = x.WarrantyMonths,

                Product = new ProductSimpleDto
                {
                    Id = x.Product.Id,
                    CategoryId = x.Product.CategoryId,
                    BrandId = x.Product.BrandId,

                    Barcode = x.Product.Barcode,
                    Slug = x.Product.Slug,

                    Name = x.Product.Name,

                    ShortDescription = x.Product.ShortDescription,
                    Description = x.Product.Description,

                    ThumbnailUrl = x.Product.ThumbnailUrl,

                    ProductType = x.Product.ProductType,

                    Category = new CategoryDto
                    {
                        Id = x.Product.Category.Id,
                        ParentId = x.Product.Category.ParentId,

                        Name = x.Product.Category.Name,
                        Slug = x.Product.Category.Slug,

                        ImageUrl = x.Product.Category.ImageUrl,
                        Description = x.Product.Category.Description,

                        IsActive = x.Product.Category.IsActive,
                        SortOrder = x.Product.Category.SortOrder,

                        Parent = x.Product.Category.Parent == null
                            ? null
                            : new CategoryParentDto
                            {
                                Id = x.Product.Category.Parent.Id,
                                Name = x.Product.Category.Parent.Name,
                                Slug = x.Product.Category.Parent.Slug
                            }
                    },

                    Brand = new BrandDto
                    {
                        Id = x.Product.Brand.Id,
                        Name = x.Product.Brand.Name,
                        LogoUrl = x.Product.Brand.LogoUrl
                    }
                }
            })
            .FirstOrDefaultAsync();

        if (variant == null)
        {
            return ApiResponse<object>.Fail(
                "Product variant not found");
        }

        return ApiResponse<object>.Ok(variant);
    }

    public async Task<ApiResponse<object>> GetByProductId(Guid productId)
    {
        var variant = await _context.ProductVariants
            .AsNoTracking()
            .Where(x => x.ProductId == productId)
            .Select(x => new ProductVariantResponse
            {
                Id = x.Id,
                ProductId = x.ProductId,
                SKU = x.SKU,
                Color = x.Color,
                Battery = x.Battery,
                MotorPower = x.MotorPower,

                ImportPrice = x.ImportPrice,
                SellingPrice = x.SellingPrice,
                WholesalePrice = x.WholesalePrice,

                StockQuantity = x.StockQuantity,
                WarrantyMonths = x.WarrantyMonths,

                Product = new ProductSimpleDto
                {
                    Id = x.Product.Id,
                    CategoryId = x.Product.CategoryId,
                    BrandId = x.Product.BrandId,

                    Barcode = x.Product.Barcode,
                    Slug = x.Product.Slug,

                    Name = x.Product.Name,

                    ShortDescription = x.Product.ShortDescription,
                    Description = x.Product.Description,

                    ThumbnailUrl = x.Product.ThumbnailUrl,

                    ProductType = x.Product.ProductType,

                    Category = new CategoryDto
                    {
                        Id = x.Product.Category.Id,
                        ParentId = x.Product.Category.ParentId,

                        Name = x.Product.Category.Name,
                        Slug = x.Product.Category.Slug,

                        ImageUrl = x.Product.Category.ImageUrl,
                        Description = x.Product.Category.Description,

                        IsActive = x.Product.Category.IsActive,
                        SortOrder = x.Product.Category.SortOrder,

                        Parent = x.Product.Category.Parent == null
                            ? null
                            : new CategoryParentDto
                            {
                                Id = x.Product.Category.Parent.Id,
                                Name = x.Product.Category.Parent.Name,
                                Slug = x.Product.Category.Parent.Slug
                            }
                    },

                    Brand = new BrandDto
                    {
                        Id = x.Product.Brand.Id,
                        Name = x.Product.Brand.Name,
                        LogoUrl = x.Product.Brand.LogoUrl
                    }
                }
            })
            .FirstOrDefaultAsync();

        if (variant == null)
        {
            return ApiResponse<object>.Fail(
                "Product variant not found");
        }

        return ApiResponse<object>.Ok(variant);
    }
    public async Task<ApiResponse<object>> Create(
        CreateProductVariantRequest request)
    {
        var errors =
            ProductVariantValidator.ValidateCreate(
                request);

        if (errors.Any())
        {
            return ApiResponse<object>.Fail(
                "Validation failed",
                errors);
        }

        // check product exists
        var product = await _context.Products
            .FirstOrDefaultAsync(x =>
                x.Id == request.ProductId);

        if (product == null)
        {
            return ApiResponse<object>.Fail(
                "Product not found",
                errors);
        }

        var nextSKU = await _counterService
                                    .GetNextAsync(CounterCodes.ProductVariantSKU);

        var SKU =
            $"PRODUCTVARIANTKU{DateTime.UtcNow.Year}{nextSKU:D5}";

        var variant = new ProductVariant
        {
            Id = Guid.NewGuid(),

            ProductId =
                request.ProductId,

            SKU =
                SKU,

            Color =
                request.Color,

            Battery =
                request.Battery,

            MotorPower =
                request.MotorPower,

            ImportPrice =
                request.ImportPrice,

            SellingPrice =
                request.SellingPrice,

            WholesalePrice =
                request.WholesalePrice,

            StockQuantity =
                request.StockQuantity,

            WarrantyMonths =
                request.WarrantyMonths,

            TrackSerial = product.ProductType is ProductType.Bicycle or ProductType.ElectricBicycle or ProductType.Motorcycle or ProductType.ElectricMotorcycle
        };

        _context.ProductVariants.Add(
            variant);

        await _context.SaveChangesAsync();

        return ApiResponse<object>.Ok(
            variant,
            "Created successfully");
    }

    public async Task<ApiResponse<object>> Update(
        Guid id,
        UpdateProductVariantRequest request)
    {
        var errors =
            ProductVariantValidator.ValidateUpdate(
                request);

        if (errors.Any())
        {
            return ApiResponse<object>.Fail(
                "Validation failed",
                errors);
        }

        var variant = await _context.ProductVariants
            .FirstOrDefaultAsync(x =>
                x.Id == id);

        if (variant == null)
        {
            return ApiResponse<object>.Fail(
                "Product variant not found",
                errors);
        }

        // check product exists
        var productExists = await _context.Products
            .AnyAsync(x =>
                x.Id == request.ProductId);

        if (!productExists)
        {
            return ApiResponse<object>.Fail(
                "Product not found",
                errors);
        }

        variant.ProductId =
            request.ProductId;

        variant.Color =
            request.Color;

        variant.Battery =
            request.Battery;

        variant.MotorPower =
            request.MotorPower;

        variant.ImportPrice =
            request.ImportPrice;

        variant.SellingPrice =
            request.SellingPrice;

        variant.WholesalePrice =
            request.WholesalePrice;

        variant.StockQuantity =
            request.StockQuantity;

        variant.WarrantyMonths =
            request.WarrantyMonths;

        await _context.SaveChangesAsync();

        return ApiResponse<object>.Ok(
            variant,
            "Updated successfully");
    }

    public async Task<ApiResponse<object>> Delete(
        Guid id)
    {
        var errors =
            ProductVariantValidator.ValidateDelete(
                id);

        if (errors.Any())
        {
            return ApiResponse<object>.Fail(
                "Validation failed",
                errors);
        }

        var variant = await _context.ProductVariants
            .FirstOrDefaultAsync(x =>
                x.Id == id);

        if (variant == null)
        {
            return ApiResponse<object>.Fail(
                "Product variant not found",
                errors);
        }

        _context.ProductVariants.Remove(
            variant);

        await _context.SaveChangesAsync();

        return ApiResponse<object>.Ok(
            null,
            "Deleted successfully");
    }

    public async Task<ApiResponse<object>> GetDashboardProduct()
    {
        var variants = await _context.ProductVariants
            .ToListAsync();
        var totalStock = variants.Sum(x => x.StockQuantity);
        var totalSellingPrice = variants.Sum(x => x.SellingPrice * x.StockQuantity);
        var result = new
        {
            TotalStock = totalStock,
            TotalSellingPrice = totalSellingPrice
        };
        return ApiResponse<object>.Ok(result);
    }


}