// Services/OrderItemService.cs
using BikeManagerV3.Order.Data;
using BikeManagerV3.Order.DTOs.OrderItems;
using BikeManagerV3.Order.DTOs.Orders;
using BikeManagerV3.Order.Models;
using BikeManagerV3.Order.Responses;
using BikeManagerV3.Product.Data;
using BikeManagerV3.Product.DTOs.Brand;
using BikeManagerV3.Product.DTOs.Category;
using BikeManagerV3.Product.DTOs.ProductVariant;
using BikeManagerV3.Product.DTOs.SerialNumber;
using BikeManagerV3.Product.Product.DTOs;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Order.Services;

public class OrderItemService
    : IOrderItemService
{
    private readonly OrderDbContext _context;
    private readonly CatalogDbContext _catalogDb;
    public OrderItemService(
        OrderDbContext context,
        CatalogDbContext catalogDbcontext)
    {
        _context = context;
        _catalogDb = catalogDbcontext;
    }

    public async Task<ApiResponse<OrderItemResponse>>
        CreateAsync(
            CreateOrderItemRequest request)
    {
        var item = new OrderItem
        {
            Id = Guid.NewGuid(),
            OrderId = request.OrderId,
            ProductVariantId =
                request.ProductVariantId,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice,
            DiscountAmount =
                request.DiscountAmount,
            TotalPrice = request.TotalPrice
        };

        _context.OrderItems.Add(item);

        await _context.SaveChangesAsync();

        return ApiResponse<OrderItemResponse>.Ok(
            Map(item),
            "Created successfully");
    }

    public async Task<PagedResult<OrderItemResponse>> GetAllAsync(
    OrderItemQuery query)
    {
        var dbQuery = _context.OrderItems
            .Include(x => x.Order)
            .AsQueryable();

        if (query.OrderId.HasValue)
        {
            dbQuery = dbQuery.Where(x =>
                x.OrderId == query.OrderId.Value);
        }

        var items = await dbQuery.ToListAsync();

        // ==========================
        // ProductVariant
        // ==========================

        var variantIds = items
            .Select(x => x.ProductVariantId)
            .Distinct()
            .ToList();

        var variants = await _catalogDb.ProductVariants
            .AsNoTracking()
            .Where(x => variantIds.Contains(x.Id))
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
            .ToDictionaryAsync(x => x.Id);

        // ==========================
        // Serial
        // ==========================

        var serialIds = items
            .Where(x => x.SerialNumberId.HasValue)
            .Select(x => x.SerialNumberId!.Value)
            .Distinct()
            .ToList();

        var serials = await _catalogDb.SerialNumbers
            .AsNoTracking()
            .Where(x => serialIds.Contains(x.Id))
            .Select(x => new SerialNumberResponse
            {
                Id = x.Id,

                SerialCode = x.SerialCode,

                FrameNumber = x.FrameNumber,

                EngineNumber = x.EngineNumber,

                BatterySerial = x.BatterySerial,

                MotorSerial = x.MotorSerial,

                QRCode = x.QRCode,

                ManufacturingDate = x.ManufacturingDate,

                ImportDate = x.ImportDate,

                WarrantyStart = x.WarrantyStart,

                WarrantyEnd = x.WarrantyEnd,

                CurrentStatus = x.CurrentStatus,

                WarehouseId = x.WarehouseId
            })
            .ToDictionaryAsync(x => x.Id);

        // ==========================
        // Response
        // ==========================

        var response = items.Select(item =>
            new OrderItemResponse
            {
                Id = item.Id,

                OrderId = item.OrderId,

                ProductVariantId = item.ProductVariantId,

                SerialNumberId = item.SerialNumberId,

                Quantity = item.Quantity,

                UnitPrice = item.UnitPrice,

                DiscountAmount = item.DiscountAmount,

                TotalPrice = item.TotalPrice,

                //Order = new OrderResponse
                //{
                //    CustomerId = item.Order.CustomerId,

                //    OrderCode = item.Order.OrderCode,

                //    SubTotal = item.Order.SubTotal,

                //    DiscountAmount = item.Order.DiscountAmount,

                //    TaxAmount = item.Order.TaxAmount,

                //    TotalAmount = item.Order.TotalAmount,

                //    PaymentMethod = item.Order.PaymentMethod,

                //    PaymentStatus = item.Order.PaymentStatus,

                //    OrderStatus = item.Order.OrderStatus,

                //    CreatedBy = item.Order.CreatedBy,

                //    CreatedAt = item.Order.CreatedAt
                //},

                ProductVariant =
                    variants.GetValueOrDefault(
                        item.ProductVariantId),

                SerialNumber =
                    item.SerialNumberId.HasValue
                        ? serials.GetValueOrDefault(
                            item.SerialNumberId.Value)
                        : null
            })
            .ToList();
        var totalItems = await dbQuery.CountAsync();

        return new PagedResult<OrderItemResponse>
        {
            Page = query.Page,
            PageSize = query.PageSize,
            TotalItems = totalItems,

            TotalPages = (int)Math.Ceiling(
                totalItems / (double)query.PageSize),

            Items = response

        };
    }

    public async Task<ApiResponse<OrderItemResponse>>
        GetByIdAsync(Guid id)
    {
        var item = await _context.OrderItems
            .FirstOrDefaultAsync(x =>
                x.Id == id);
        var variant = await _catalogDb.ProductVariants
            .AsNoTracking()
            .Where(x => x.Id == item.ProductVariantId)
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
        var serial = await _catalogDb.SerialNumbers
            .AsNoTracking()
            .Where(x => x.Id == item.SerialNumberId)
            .Select(x => new SerialNumberResponse
            {
                Id = x.Id,
                SerialCode = x.SerialCode,
                FrameNumber = x.FrameNumber,
                EngineNumber = x.EngineNumber,
                BatterySerial = x.BatterySerial,
                MotorSerial = x.MotorSerial,
                QRCode = x.QRCode,
                ManufacturingDate = x.ManufacturingDate,
                ImportDate = x.ImportDate,
                WarrantyStart = x.WarrantyStart,
                WarrantyEnd = x.WarrantyEnd,
                CurrentStatus = x.CurrentStatus,
                WarehouseId = x.WarehouseId,
            })
            .FirstOrDefaultAsync();
        if (item == null)
        {
            return ApiResponse<OrderItemResponse>
                .Fail("Order item not found");
        }

        return ApiResponse<OrderItemResponse>
            .Ok(new OrderItemResponse
            {
                Id = item.Id,
                OrderId = item.OrderId,
                ProductVariantId = item.ProductVariantId,
                SerialNumberId = item.SerialNumberId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                DiscountAmount = item.DiscountAmount,
                TotalPrice = item.TotalPrice,
                Order = new OrderResponse
                {
                    CustomerId = item.Order.CustomerId,
                    OrderCode = item.Order.OrderCode,
                    SubTotal = item.Order.SubTotal,
                    DiscountAmount = item.Order.DiscountAmount,
                    TaxAmount = item.Order.TaxAmount,
                    TotalAmount = item.Order.TotalAmount,
                    PaymentMethod = item.Order.PaymentMethod,
                    PaymentStatus = item.Order.PaymentStatus,
                    OrderStatus = item.Order.OrderStatus,
                    CreatedBy = item.Order.CreatedBy,
                    CreatedAt = item.Order.CreatedAt,
                },
                ProductVariant = variant,
                SerialNumber = serial
            });
    }

    public async Task<ApiResponse<OrderItemResponse>>
        UpdateAsync(
            Guid id,
            UpdateOrderItemRequest request)
    {
        var item = await _context.OrderItems
            .FirstOrDefaultAsync(x =>
                x.Id == id);

        if (item == null)
        {
            return ApiResponse<OrderItemResponse>
                .Fail("Order item not found");
        }

        item.ProductVariantId =
            request.ProductVariantId;

        item.Quantity =
            request.Quantity;

        item.UnitPrice =
            request.UnitPrice;

        item.DiscountAmount =
            request.DiscountAmount;

        item.TotalPrice =
            request.TotalPrice;

        await _context.SaveChangesAsync();

        return ApiResponse<OrderItemResponse>
            .Ok(
                Map(item),
                "Updated successfully");
    }

    public async Task<ApiResponse<string>>
        DeleteAsync(Guid id)
    {
        var item = await _context.OrderItems
            .FirstOrDefaultAsync(x =>
                x.Id == id);

        if (item == null)
        {
            return ApiResponse<string>
                .Fail("Order item not found");
        }

        _context.OrderItems.Remove(item);

        await _context.SaveChangesAsync();

        return ApiResponse<string>.Ok(
            "Deleted",
            "Deleted successfully");
    }

    private static OrderItemResponse Map(
        OrderItem item)
    {
        return new OrderItemResponse
        {
            Id = item.Id,
            OrderId = item.OrderId,
            ProductVariantId =
                item.ProductVariantId,
            SerialNumberId =
                item.SerialNumberId,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            DiscountAmount =
                item.DiscountAmount,
            TotalPrice = item.TotalPrice
        };
    }
    //private static OrderItemResponse MapDetail(
    //   OrderItem item)
    //{
    //    return new OrderItemResponse
    //    {
    //        Id = item.Id,
    //        OrderId = item.OrderId,
    //        ProductVariantId = item.ProductVariantId,
    //        SerialNumberId = item.SerialNumberId,
    //        Quantity = item.Quantity,
    //        UnitPrice = item.UnitPrice,
    //        DiscountAmount = item.DiscountAmount,
    //        TotalPrice = item.TotalPrice,
    //        Order = new OrderResponse
    //        {
    //            CustomerId = item.Order.CustomerId,
    //            OrderCode = item.Order.OrderCode,
    //            SubTotal = item.Order.SubTotal,
    //            DiscountAmount = item.Order.DiscountAmount,
    //            TaxAmount = item.Order.TaxAmount,
    //            TotalAmount = item.Order.TotalAmount,
    //            PaymentMethod = item.Order.PaymentMethod,
    //            PaymentStatus = item.Order.PaymentStatus,
    //            OrderStatus = item.Order.OrderStatus,
    //            CreatedBy = item.Order.CreatedBy,
    //            CreatedAt = item.Order.CreatedAt,
    //        },
    //        ProductVariant = new ProductVariantResponse
    //        {
    //            Id = item.ProductVariant.Id,
    //            ProductId = item.ProductVariant.ProductId,
    //            SKU = item.ProductVariant.SKU,
    //            Color = item.ProductVariant.Color,
    //            Battery = item.ProductVariant.Battery,
    //            MotorPower = item.ProductVariant.MotorPower,
    //            ImportPrice = item.ProductVariant.ImportPrice,
    //            SellingPrice = item.ProductVariant.SellingPrice,
    //            WholesalePrice = item.ProductVariant.WholesalePrice,
    //            StockQuantity = item.ProductVariant.StockQuantity,
    //            WarrantyMonths = item.ProductVariant.WarrantyMonths,
    //            TrackSerial = item.ProductVariant.TrackSerial,
    //            Product = new ProductSimpleDto
    //            {
    //                Id = item.ProductVariant.Product.Id,
    //                CategoryId = item.ProductVariant.Product.CategoryId,
    //                BrandId = item.ProductVariant.Product.BrandId,
    //                SKU = item.ProductVariant.Product.SKU,
    //                Barcode = item.ProductVariant.Product.Barcode,
    //                Name = item.ProductVariant.Product.Name,
    //                Slug = item.ProductVariant.Product.Slug,
    //                ShortDescription = item.ProductVariant.Product.ShortDescription,
    //                Description = item.ProductVariant.Product.Description,
    //                ThumbnailUrl = item.ProductVariant.Product.ThumbnailUrl,
    //                ProductType = item.ProductVariant.Product.ProductType,
    //                Category = new CategoryDto
    //                {
    //                    Id = item.ProductVariant.Product.Category.Id,
    //                    ParentId = item.ProductVariant.Product.Category.ParentId,
    //                    Name = item.ProductVariant.Product.Category.Name,
    //                    Slug = item.ProductVariant.Product.Category.Slug,
    //                    ImageUrl = item.ProductVariant.Product.Category.ImageUrl,
    //                    Description = item.ProductVariant.Product.Category.Description,
    //                    IsActive = item.ProductVariant.Product.Category.IsActive,
    //                },
    //                Brand = new BrandDto
    //                {
    //                    Id = item.ProductVariant.Product.Brand.Id,
    //                    Name = item.ProductVariant.Product.Brand.Name,
    //                    LogoUrl = item.ProductVariant.Product.Brand.LogoUrl,
    //                }
    //            }
    //        },
    //        SerialNumber = new SerialNumberResponse
    //        {
    //            Id = item.SerialNumber.Id,
    //            SerialCode = item.SerialNumber.SerialCode,
    //            FrameNumber = item.SerialNumber.FrameNumber,
    //            EngineNumber = item.SerialNumber.EngineNumber,
    //            BatterySerial = item.SerialNumber.BatterySerial,
    //            MotorSerial = item.SerialNumber.MotorSerial,
    //            QRCode = item.SerialNumber.QRCode,
    //            ManufacturingDate = item.SerialNumber.ManufacturingDate,
    //            ImportDate = item.SerialNumber.ImportDate,
    //            WarrantyStart = item.SerialNumber.WarrantyStart,
    //            WarrantyEnd = item.SerialNumber.WarrantyEnd,
    //            CurrentStatus = item.SerialNumber.CurrentStatus,
    //            WarehouseId = item.SerialNumber.WarehouseId,
    //        }
    //    };
    //}
}