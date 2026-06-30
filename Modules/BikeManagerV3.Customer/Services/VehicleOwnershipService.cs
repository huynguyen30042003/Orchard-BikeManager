// Services/VehicleOwnershipService.cs
using BikeManagerV3.Customer.Data;
using BikeManagerV3.Customer.DTOs.VehicleOwnerships;
using BikeManagerV3.Customer.Models;
using BikeManagerV3.Customer.Responses;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Customer.Services;

public class VehicleOwnershipService
    : IVehicleOwnershipService
{
    private readonly CustomerDbContext _context;

    public VehicleOwnershipService(
        CustomerDbContext context)
    {
        _context = context;
    }

    public async Task<
        ApiResponse<VehicleOwnershipResponse>>
        CreateAsync(
            CreateVehicleOwnershipRequest request)
    {
        var ownership = new VehicleOwnership
        {
            Id = Guid.NewGuid(),
            SerialNumberId =
                request.SerialNumberId,
            CustomerId = request.CustomerId,
            OrderId = request.OrderId,
            OwnershipStart =
                request.OwnershipStart,
            OwnershipEnd =
                request.OwnershipEnd,
            IsCurrentOwner =
                request.IsCurrentOwner
        };

        _context.VehicleOwnerships
            .Add(ownership);

        await _context.SaveChangesAsync();

        return ApiResponse<
            VehicleOwnershipResponse>.Ok(
                Map(ownership),
                "Created successfully");
    }

    public async Task<
        ApiResponse<List<VehicleOwnershipResponse>>>
        GetAllAsync(VehicleOwnershipQuery query)
    {
        var dbQuery = _context
            .VehicleOwnerships
            .AsQueryable();

        if (query.CustomerId.HasValue)
        {
            dbQuery = dbQuery.Where(x =>
                x.CustomerId ==
                query.CustomerId.Value);
        }

        if (query.IsCurrentOwner.HasValue)
        {
            dbQuery = dbQuery.Where(x =>
                x.IsCurrentOwner ==
                query.IsCurrentOwner.Value);
        }

        var items = await dbQuery
            .Skip((query.Page - 1) *
                query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return ApiResponse<
            List<VehicleOwnershipResponse>>
            .Ok(
                items.Select(Map).ToList());
    }

    public async Task<
        ApiResponse<VehicleOwnershipResponse>>
        GetByIdAsync(Guid id)
    {
        var item = await _context
            .VehicleOwnerships
            .FirstOrDefaultAsync(x =>
                x.Id == id);

        if (item == null)
        {
            return ApiResponse<
                VehicleOwnershipResponse>
                .Fail("Ownership not found");
        }

        return ApiResponse<
            VehicleOwnershipResponse>
            .Ok(Map(item));
    }

    public async Task<
        ApiResponse<VehicleOwnershipResponse>>
        UpdateAsync(
            Guid id,
            UpdateVehicleOwnershipRequest request)
    {
        var item = await _context
            .VehicleOwnerships
            .FirstOrDefaultAsync(x =>
                x.Id == id);

        if (item == null)
        {
            return ApiResponse<
                VehicleOwnershipResponse>
                .Fail("Ownership not found");
        }

        item.SerialNumberId =
            request.SerialNumberId;

        item.CustomerId =
            request.CustomerId;

        item.OrderId =
            request.OrderId;

        item.OwnershipStart =
            request.OwnershipStart;

        item.OwnershipEnd =
            request.OwnershipEnd;

        item.IsCurrentOwner =
            request.IsCurrentOwner;

        await _context.SaveChangesAsync();

        return ApiResponse<
            VehicleOwnershipResponse>
            .Ok(
                Map(item),
                "Updated successfully");
    }

    public async Task<ApiResponse<string>>
        DeleteAsync(Guid id)
    {
        var item = await _context
            .VehicleOwnerships
            .FirstOrDefaultAsync(x =>
                x.Id == id);

        if (item == null)
        {
            return ApiResponse<string>
                .Fail("Ownership not found");
        }

        _context.VehicleOwnerships
            .Remove(item);

        await _context.SaveChangesAsync();

        return ApiResponse<string>.Ok(
            "Deleted",
            "Deleted successfully");
    }

    private static VehicleOwnershipResponse Map(
        VehicleOwnership item)
    {
        return new VehicleOwnershipResponse
        {
            Id = item.Id,
            SerialNumberId =
                item.SerialNumberId,
            CustomerId = item.CustomerId,
            OrderId = item.OrderId,
            OwnershipStart =
                item.OwnershipStart,
            OwnershipEnd =
                item.OwnershipEnd,
            IsCurrentOwner =
                item.IsCurrentOwner
        };
    }
}