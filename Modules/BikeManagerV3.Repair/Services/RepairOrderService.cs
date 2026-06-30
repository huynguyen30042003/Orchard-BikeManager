using BikeManagerV3.Product.Constants;
using BikeManagerV3.Product.Services;
using BikeManagerV3.Repair.Data;
using BikeManagerV3.Repair.DTOs.RepairOrders;
using BikeManagerV3.Repair.Models;
using BikeManagerV3.Repair.Responses;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Repair.Services;

public class RepairOrderService
    : IRepairOrderService
{
    private readonly RepairDbContext _context;
    private readonly ICounterService _counterService;

    public RepairOrderService(
        RepairDbContext context, ICounterService counterService)
    {
        _context = context;
        _counterService = counterService;

    }

    public async Task<
        ApiResponse<RepairOrderResponse>>
        CreateAsync(
            CreateRepairOrderRequest request)
    {
        var next = await _counterService
                            .GetNextAsync(CounterCodes.RepairOrder);

        var RepairCode =
            $"REPAIRORDER{DateTime.UtcNow.Year}{next:D5}";
        var order = new RepairOrder
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            CustomerVehicleId =
                request.CustomerVehicleId,
            RepairCode = RepairCode,
            IssueDescription =
                request.IssueDescription,
            Diagnosis = request.Diagnosis,
            Status = request.Status,
            EstimatedCost =
                request.EstimatedCost,
            TotalCost = request.TotalCost,
            CheckInAt = request.CheckInAt,
            CompletedAt =
                request.CompletedAt
        };

        _context.RepairOrders.Add(order);

        await _context.SaveChangesAsync();

        return ApiResponse<
            RepairOrderResponse>.Ok(
                Map(order),
                "Created successfully");
    }

    public async Task<
        PagedResult<RepairOrderResponse>>
        GetAllAsync(
            RepairOrderQuery query)
    {
        var dbQuery = _context.RepairOrders
            .AsQueryable();

        if (query.CustomerId.HasValue)
        {
            dbQuery = dbQuery.Where(x =>
                x.CustomerId ==
                query.CustomerId.Value);
        }

        if (!string.IsNullOrWhiteSpace(
            query.Status))
        {
            dbQuery = dbQuery.Where(x =>
                x.Status == query.Status);
        }
        var totalItems = await dbQuery.CountAsync();

        var orders = await dbQuery
            .Skip((query.Page - 1) *
                query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();


        return new PagedResult<RepairOrderResponse>
        {
            Page = query.Page,
            PageSize = query.PageSize,
            TotalItems = totalItems,

            TotalPages = (int)Math.Ceiling(
                totalItems / (double)query.PageSize),

            Items = orders.Select(Map).ToList()

        };
    }

    public async Task<
        ApiResponse<RepairOrderResponse>>
        GetByIdAsync(Guid id)
    {
        var order = await _context
            .RepairOrders
            .FirstOrDefaultAsync(x =>
                x.Id == id);

        if (order == null)
        {
            return ApiResponse<
                RepairOrderResponse>
                .Fail(
                    "Repair order not found");
        }

        return ApiResponse<
            RepairOrderResponse>
            .Ok(Map(order));
    }

    public async Task<
        ApiResponse<RepairOrderResponse>>
        UpdateAsync(
            Guid id,
            UpdateRepairOrderRequest request)
    {
        var order = await _context
            .RepairOrders
            .FirstOrDefaultAsync(x =>
                x.Id == id);

        if (order == null)
        {
            return ApiResponse<
                RepairOrderResponse>
                .Fail(
                    "Repair order not found");
        }

        order.RepairCode =
            request.RepairCode;

        order.IssueDescription =
            request.IssueDescription;

        order.Diagnosis =
            request.Diagnosis;

        order.Status =
            request.Status;

        order.EstimatedCost =
            request.EstimatedCost;

        order.TotalCost =
            request.TotalCost;

        order.CheckInAt =
            request.CheckInAt;

        order.CompletedAt =
            request.CompletedAt;

        await _context.SaveChangesAsync();

        return ApiResponse<
            RepairOrderResponse>
            .Ok(
                Map(order),
                "Updated successfully");
    }

    public async Task<
        ApiResponse<string>>
        DeleteAsync(Guid id)
    {
        var order = await _context
            .RepairOrders
            .FirstOrDefaultAsync(x =>
                x.Id == id);

        if (order == null)
        {
            return ApiResponse<string>
                .Fail(
                    "Repair order not found");
        }

        _context.RepairOrders
            .Remove(order);

        await _context.SaveChangesAsync();

        return ApiResponse<string>.Ok(
            "Deleted",
            "Deleted successfully");
    }

    private static RepairOrderResponse Map(
        RepairOrder order)
    {
        return new RepairOrderResponse
        {
            Id = order.Id,
            CustomerId =
                order.CustomerId,
            CustomerVehicleId =
                order.CustomerVehicleId,
            RepairCode =
                order.RepairCode,
            IssueDescription =
                order.IssueDescription,
            Diagnosis =
                order.Diagnosis,
            Status = order.Status,
            EstimatedCost =
                order.EstimatedCost,
            TotalCost =
                order.TotalCost,
            CheckInAt =
                order.CheckInAt,
            CompletedAt =
                order.CompletedAt
        };
    }
}