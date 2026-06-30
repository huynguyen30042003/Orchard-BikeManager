// Services/CustomerVehicleService.cs
using BikeManagerV3.Customer.Data;
using BikeManagerV3.Customer.DTOs.Customers;
using BikeManagerV3.Customer.DTOs.CustomerVehicles;
using BikeManagerV3.Customer.Models;
using BikeManagerV3.Customer.Responses;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Customer.Services;

public class CustomerVehicleService
    : ICustomerVehicleService
{
    private readonly CustomerDbContext _context;

    public CustomerVehicleService(
        CustomerDbContext context)
    {
        _context = context;
    }

    public async Task<
        ApiResponse<CustomerVehicleResponse>>
        CreateAsync(
            CreateCustomerVehicleRequest request)
    {
        var vehicle = new CustomerVehicle
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            BrandId = request.BrandId,
            ModelName = request.ModelName,
            PlateNumber = request.PlateNumber,
            FrameNumber = request.FrameNumber,
            EngineNumber = request.EngineNumber,
            BatterySerial = request.BatterySerial,
            PurchaseDate = request.PurchaseDate
        };

        _context.CustomerVehicles.Add(vehicle);

        await _context.SaveChangesAsync();

        return ApiResponse<
            CustomerVehicleResponse>.Ok(
                Map(vehicle),
                "Created successfully");
    }

    public async Task<
        PagedResult<CustomerVehicleResponse>>
        GetAllAsync(CustomerVehicleQuery query)
    {
        var dbQuery = _context.CustomerVehicles
            .AsQueryable();

        if (query.CustomerId.HasValue)
        {
            dbQuery = dbQuery.Where(x =>
                x.CustomerId ==
                query.CustomerId.Value);
        }

        if (!string.IsNullOrWhiteSpace(
            query.Search))
        {
            dbQuery = dbQuery.Where(x =>
                x.ModelName.Contains(
                    query.Search) ||
                x.PlateNumber.Contains(
                    query.Search));
        }

        var vehicles = await dbQuery
            .Skip((query.Page - 1) *
                query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();
        var totalItems = await dbQuery.CountAsync();

        return new PagedResult<CustomerVehicleResponse>
        {
            Page = query.Page,
            PageSize = query.PageSize,
            TotalItems = totalItems,

            TotalPages = (int)Math.Ceiling(
                totalItems / (double)query.PageSize),

            Items = vehicles.Select(Map).ToList()

        };
        //return ApiResponse<
        //    List<CustomerVehicleResponse>>
        //    .Ok(
        //        vehicles.Select(Map).ToList());
    }

    public async Task<
        ApiResponse<CustomerVehicleResponse>>
        GetByIdAsync(Guid id)
    {
        var vehicle = await _context
            .CustomerVehicles
            .FirstOrDefaultAsync(x =>
                x.Id == id);

        if (vehicle == null)
        {
            return ApiResponse<
                CustomerVehicleResponse>
                .Fail("Vehicle not found");
        }

        return ApiResponse<
            CustomerVehicleResponse>
            .Ok(Map(vehicle));
    }

    public async Task<
        ApiResponse<CustomerVehicleResponse>>
        UpdateAsync(
            Guid id,
            UpdateCustomerVehicleRequest request)
    {
        var vehicle = await _context
            .CustomerVehicles
            .FirstOrDefaultAsync(x =>
                x.Id == id);

        if (vehicle == null)
        {
            return ApiResponse<
                CustomerVehicleResponse>
                .Fail("Vehicle not found");
        }

        vehicle.BrandId = request.BrandId;
        vehicle.ModelName = request.ModelName;
        vehicle.PlateNumber = request.PlateNumber;
        vehicle.FrameNumber = request.FrameNumber;
        vehicle.EngineNumber = request.EngineNumber;
        vehicle.BatterySerial =
            request.BatterySerial;
        vehicle.PurchaseDate =
            request.PurchaseDate;

        await _context.SaveChangesAsync();

        return ApiResponse<
            CustomerVehicleResponse>
            .Ok(
                Map(vehicle),
                "Updated successfully");
    }

    public async Task<ApiResponse<string>>
        DeleteAsync(Guid id)
    {
        var vehicle = await _context
            .CustomerVehicles
            .FirstOrDefaultAsync(x =>
                x.Id == id);

        if (vehicle == null)
        {
            return ApiResponse<string>
                .Fail("Vehicle not found");
        }

        _context.CustomerVehicles
            .Remove(vehicle);

        await _context.SaveChangesAsync();

        return ApiResponse<string>.Ok(
            "Deleted",
            "Deleted successfully");
    }

    private static CustomerVehicleResponse Map(
        CustomerVehicle vehicle)
    {
        return new CustomerVehicleResponse
        {
            Id = vehicle.Id,
            CustomerId = vehicle.CustomerId,
            BrandId = vehicle.BrandId,
            ModelName = vehicle.ModelName,
            PlateNumber = vehicle.PlateNumber,
            FrameNumber = vehicle.FrameNumber,
            EngineNumber = vehicle.EngineNumber,
            BatterySerial = vehicle.BatterySerial,
            PurchaseDate = vehicle.PurchaseDate
        };
    }
}