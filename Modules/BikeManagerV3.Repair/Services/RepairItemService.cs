using BikeManagerV3.Repair.Data;
using BikeManagerV3.Repair.DTOs.RepairItems;
using BikeManagerV3.Repair.Models;
using BikeManagerV3.Repair.Responses;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Repair.Services;

public class RepairItemService
    : IRepairItemService
{
    private readonly RepairDbContext _context;

    public RepairItemService(
        RepairDbContext context)
    {
        _context = context;
    }

    public async Task<
        ApiResponse<RepairItemResponse>>
        CreateAsync(
            CreateRepairItemRequest request)
    {
        var item = new RepairItem
        {
            Id = Guid.NewGuid(),
            RepairOrderId =
                request.RepairOrderId,
            ItemType = request.ItemType,
            ProductVariantId =
                request.ProductVariantId,
            ServiceId =
                request.ServiceId,
            Quantity = request.Quantity,
            UnitPrice =
                request.UnitPrice,
            TotalPrice =
                request.TotalPrice
        };

        _context.RepairItems.Add(item);

        await _context.SaveChangesAsync();

        return ApiResponse<
            RepairItemResponse>.Ok(
                Map(item),
                "Created successfully");
    }

    public async Task<
        ApiResponse<List<RepairItemResponse>>>
        GetAllAsync(
            RepairItemQuery query)
    {
        var dbQuery = _context.RepairItems
            .AsQueryable();

        if (query.RepairOrderId.HasValue)
        {
            dbQuery = dbQuery.Where(x =>
                x.RepairOrderId ==
                query.RepairOrderId.Value);
        }

        if (!string.IsNullOrWhiteSpace(
            query.ItemType))
        {
            dbQuery = dbQuery.Where(x =>
                x.ItemType ==
                query.ItemType);
        }

        var items = await dbQuery
            .Skip((query.Page - 1) *
                query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return ApiResponse<
            List<RepairItemResponse>>
            .Ok(
                items.Select(Map).ToList());
    }

    public async Task<
        ApiResponse<RepairItemResponse>>
        GetByIdAsync(Guid id)
    {
        var item = await _context
            .RepairItems
            .FirstOrDefaultAsync(x =>
                x.Id == id);

        if (item == null)
        {
            return ApiResponse<
                RepairItemResponse>
                .Fail(
                    "Repair item not found");
        }

        return ApiResponse<
            RepairItemResponse>
            .Ok(Map(item));
    }

    public async Task<
        ApiResponse<RepairItemResponse>>
        UpdateAsync(
            Guid id,
            UpdateRepairItemRequest request)
    {
        var item = await _context
            .RepairItems
            .FirstOrDefaultAsync(x =>
                x.Id == id);

        if (item == null)
        {
            return ApiResponse<
                RepairItemResponse>
                .Fail(
                    "Repair item not found");
        }

        item.ItemType =
            request.ItemType;

        item.ProductVariantId =
            request.ProductVariantId;

        item.ServiceId =
            request.ServiceId;

        item.Quantity =
            request.Quantity;

        item.UnitPrice =
            request.UnitPrice;

        item.TotalPrice =
            request.TotalPrice;

        await _context.SaveChangesAsync();

        return ApiResponse<
            RepairItemResponse>
            .Ok(
                Map(item),
                "Updated successfully");
    }

    public async Task<
        ApiResponse<string>>
        DeleteAsync(Guid id)
    {
        var item = await _context
            .RepairItems
            .FirstOrDefaultAsync(x =>
                x.Id == id);

        if (item == null)
        {
            return ApiResponse<string>
                .Fail(
                    "Repair item not found");
        }

        _context.RepairItems
            .Remove(item);

        await _context.SaveChangesAsync();

        return ApiResponse<string>.Ok(
            "Deleted",
            "Deleted successfully");
    }

    private static RepairItemResponse Map(
        RepairItem item)
    {
        return new RepairItemResponse
        {
            Id = item.Id,
            RepairOrderId =
                item.RepairOrderId,
            ItemType =
                item.ItemType,
            ProductVariantId =
                item.ProductVariantId,
            ServiceId =
                item.ServiceId,
            Quantity =
                item.Quantity,
            UnitPrice =
                item.UnitPrice,
            TotalPrice =
                item.TotalPrice
        };
    }
}