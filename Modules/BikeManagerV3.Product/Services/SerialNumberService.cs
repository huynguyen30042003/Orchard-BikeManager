using Azure.Core;
using BikeManagerV3.Counters.Services;
using BikeManagerV3.Product.Constants;
using BikeManagerV3.Product.Data;
using BikeManagerV3.Product.DTOs.ProductVariant;
using BikeManagerV3.Product.DTOs.SerialNumber;
using BikeManagerV3.Product.Enums;
using BikeManagerV3.Product.Models;
using BikeManagerV3.Product.Product.DTOs;
using BikeManagerV3.Product.Responses;
using BikeManagerV3.Product.Services.Interfaces;
using BikeManagerV3.Product.Validators;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Product.Services;

public class SerialNumberService
    : ISerialNumberService
{
    private readonly ICounterService _counterService;
    private readonly CatalogDbContext _context;

    public SerialNumberService(
        CatalogDbContext context, ICounterService counterService)
    {
        _context = context;
        _counterService = counterService;
    }

    public async Task<PagedResult<SerialNumberResponse>> GetAll(
        SerialNumberQuery query)
    {
        const string Collation = "Vietnamese_100_CI_AI";
        var serials = _context.SerialNumbers
            .Include(x => x.ProductVariant)
            .Include(x => x.ProductVariant.Product)
            .AsQueryable();
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var keyword = query.Search.Trim();

            switch (query.SearchBy?.ToLower())
            {
                case "SerialCode":
                    serials = serials.Where(x =>
                        EF.Functions.Collate(
                            x.SerialCode,
                            Collation
                        ).Contains(keyword));

                    break;

                case "brand":
                    serials = serials.Where(x =>
                        EF.Functions.Collate(
                            x.ProductVariant.Product.Brand.Name,
                            Collation
                        ).Contains(keyword));

                    break;

                case "name":
                    serials = serials.Where(x =>
                        x.ProductVariant.Product.Name != null &&

                        EF.Functions.Collate(
                            x.ProductVariant.Product.Name,
                            Collation
                        ).Contains(keyword));

                    break;

                case "color":
                    serials = serials.Where(x =>
                        x.ProductVariant.Color != null &&

                        EF.Functions.Collate(
                            x.ProductVariant.Color,
                            Collation
                        ).Contains(keyword));

                    break;

                default:
                    serials = serials.Where(x =>

                        EF.Functions.Collate(
                            x.SerialCode,
                            Collation
                        ).Contains(keyword)

                        ||
                        x.EngineNumber != null &&
                        EF.Functions.Collate(
                            x.EngineNumber,
                            Collation
                        ).Contains(keyword)

                        ||
                        x.FrameNumber != null &&
                        EF.Functions.Collate(
                            x.FrameNumber,
                            Collation
                        ).Contains(keyword)

                        ||

                        EF.Functions.Collate(
                            x.ProductVariant.Product.Brand.Name,
                            Collation
                        ).Contains(keyword)

                        ||

                        (x.ProductVariant.Product.Name != null &&

                         EF.Functions.Collate(
                             x.ProductVariant.Product.Name,
                             Collation
                         ).Contains(keyword))

                        ||

                        (x.ProductVariant.Color != null &&

                         EF.Functions.Collate(
                             x.ProductVariant.Color,
                             Collation
                         ).Contains(keyword))
                    );

                    break;
            }
        }

        // filter variant
        if (query.ProductVariantId.HasValue)
        {
            serials = serials.Where(x =>
                x.ProductVariantId ==
                query.ProductVariantId.Value);
        }

        // filter serial code
        if (!string.IsNullOrWhiteSpace(
                query.SerialCode))
        {
            serials = serials.Where(x =>
                x.SerialCode.Contains(
                    query.SerialCode));
        }

        // filter status
        if (query.CurrentStatus.HasValue)
        {
            serials = serials.Where(x =>
                x.CurrentStatus ==
                query.CurrentStatus.Value);
        }

        // filter warehouse
        if (query.WarehouseId.HasValue)
        {
            serials = serials.Where(x =>
                x.WarehouseId ==
                query.WarehouseId.Value);
        }

        if (query.TrackSerial.HasValue)
        {
            serials = serials.Where(x =>
                x.ProductVariant.TrackSerial ==
                query.TrackSerial.Value);
        }
        var totalItems = await serials.CountAsync();

        var result = await serials
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .OrderByDescending(x => x.ImportDate)
            .Select(x => new SerialNumberResponse
            {
                Id = x.Id,
                ProductVariantId = x.ProductVariantId,
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
                ProductVariant = new ProductVariantResponse
                {
                    Id = x.ProductVariant.Id,
                    ProductId = x.ProductVariant.ProductId,
                    SKU = x.ProductVariant.SKU,
                    Color = x.ProductVariant.Color,
                    Battery = x.ProductVariant.Battery,
                    MotorPower = x.ProductVariant.MotorPower,
                    SellingPrice = x.ProductVariant.SellingPrice,
                    StockQuantity = x.ProductVariant.StockQuantity,
                    WarrantyMonths = x.ProductVariant.WarrantyMonths,
                    TrackSerial = x.ProductVariant.TrackSerial,
                    Product = new ProductSimpleDto
                    {
                        Id = x.ProductVariant.Product.Id,
                        Name = x.ProductVariant.Product.Name,
                    }
                },
            })
            .ToListAsync();

        return new PagedResult<SerialNumberResponse>
        {
            Page = query.Page,
            PageSize = query.PageSize,
            TotalItems = totalItems,

            TotalPages = (int)Math.Ceiling(
               totalItems / (double)query.PageSize),

            Items = result
        };
    }

    public async Task<ApiResponse<object>> GetById(
        Guid id)
    {
        var serial = await _context.SerialNumbers
            .FirstOrDefaultAsync(x => x.Id == id);

        if (serial == null)
        {
            return ApiResponse<object>.Fail(
                "Serial number not found");
        }

        return ApiResponse<object>.Ok(serial);
    }

    public async Task<ApiResponse<object>> CreateSerial(
        CreateSerialNumberRequest request)
    {
        var errors = SerialNumberValidator.ValidateCreate(request);

        if (errors.Any())
        {
            return ApiResponse<object>.Fail(
                "Validation failed",
                errors);
        }

        var variant = await _context.ProductVariants
            .Include(x => x.Product)
            .FirstOrDefaultAsync(x =>
                x.Id == request.ProductVariantId);

        if (variant == null)
        {
            return ApiResponse<object>.Fail(
                "Product variant not found");
        }
        var next = await _counterService
                            .GetNextAsync(CounterCodes.Serial);

        var serialCode =
            $"SERIAL{DateTime.UtcNow.Year}{next:D5}";
        var serial = new SerialNumber
        {
            Id = Guid.NewGuid(),

            ProductVariantId = request.ProductVariantId,

            SerialCode = serialCode,

            FrameNumber = request.FrameNumber,

            EngineNumber = request.EngineNumber,

            BatterySerial = request.BatterySerial,

            MotorSerial = request.MotorSerial,

            QRCode = request.QRCode,

            ManufacturingDate = request.ManufacturingDate,

            ImportDate = request.ImportDate,

            WarrantyStart = request.WarrantyStart,

            WarrantyEnd = request.WarrantyEnd,

            CurrentStatus =
                request.CurrentStatus ??
                CurrentStatus.IN_STOCK,

            WarehouseId = request.WarehouseId
        };

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            _context.SerialNumbers.Add(serial);
            if (
                variant.Product.ProductType != ProductType.Part &&
                variant.Product.ProductType != ProductType.Accessory
            )
            {
                variant.StockQuantity++;
            }

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return ApiResponse<object>.Ok(
                serial,
                "Created successfully");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            return ApiResponse<object>.Fail(
                $"Create failed: {ex.Message}");
        }
    }

    public async Task<ApiResponse<object>> Update(
        Guid id,
        UpdateSerialNumberRequest request)
    {
        var errors =
            SerialNumberValidator.ValidateUpdate(
                request);

        if (errors.Any())
        {
            return ApiResponse<object>.Fail(
                "Validation failed",
                errors);
        }

        var serial = await _context.SerialNumbers
            .FirstOrDefaultAsync(x => x.Id == id);

        if (serial == null)
        {
            return ApiResponse<object>.Fail(
                "Serial number not found",
                errors);
        }

        // check variant
        var variantExists = await _context.ProductVariants
            .AnyAsync(x =>
                x.Id == request.ProductVariantId);

        if (!variantExists)
        {
            return ApiResponse<object>.Fail(
                "Product variant not found",
                errors);
        }

        // check duplicate serial
        var serialExists = await _context.SerialNumbers
            .AnyAsync(x =>
                x.SerialCode ==
                request.SerialCode &&
                x.Id != id);

        if (serialExists)
        {
            return ApiResponse<object>.Fail(
                "Serial code already exists",
                errors);
        }

        serial.ProductVariantId =
            request.ProductVariantId;

        serial.SerialCode =
            request.SerialCode;

        serial.FrameNumber =
            request.FrameNumber;

        serial.EngineNumber =
            request.EngineNumber;

        serial.BatterySerial =
            request.BatterySerial;

        serial.MotorSerial =
            request.MotorSerial;

        serial.QRCode =
            request.QRCode;

        serial.ManufacturingDate =
            request.ManufacturingDate;

        serial.ImportDate =
            request.ImportDate;

        serial.WarrantyStart =
            request.WarrantyStart;

        serial.WarrantyEnd =
            request.WarrantyEnd;

        serial.CurrentStatus =
            request.CurrentStatus;

        serial.WarehouseId =
            request.WarehouseId;

        await _context.SaveChangesAsync();

        return ApiResponse<object>.Ok(
            serial,
            "Updated successfully");
    }

    public async Task<ApiResponse<object>> Delete(Guid id)
    {
        var serial = await _context.SerialNumbers
            .Include(x => x.ProductVariant)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (serial == null)
        {
            return ApiResponse<object>.Fail(
                "Serial number not found");
        }

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            var productType =
                serial.ProductVariant.Product.ProductType;

            var useSerial =
                productType is ProductType.Bicycle
                or ProductType.ElectricBicycle
                or ProductType.Motorcycle
                or ProductType.ElectricMotorcycle;

            if (useSerial)
            {
                serial.ProductVariant.StockQuantity--;
            }

            _context.SerialNumbers.Remove(serial);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return ApiResponse<object>.Ok(
                null,
                "Deleted successfully");
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}