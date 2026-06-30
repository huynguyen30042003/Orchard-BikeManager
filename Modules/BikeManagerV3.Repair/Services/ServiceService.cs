using BikeManagerV3.Product.Constants;
using BikeManagerV3.Product.Services;
using BikeManagerV3.Repair.Data;
using BikeManagerV3.Repair.DTOs.Services;
using BikeManagerV3.Repair.Responses;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Repair.Services;

public class ServiceService : IServiceService
{
    private readonly RepairDbContext _context;
    private readonly ICounterService _counterService;

    public ServiceService(
        RepairDbContext context, ICounterService counterService)
    {
        _context = context;
        _counterService = counterService;
    }

    public async Task<ApiResponse<ServiceResponse>>
        CreateAsync(
            CreateServiceRequest request)
    {
        var next = await _counterService
                            .GetNextAsync(CounterCodes.RepairService);

        var ServiceCode =
            $"REPAIRSERVICE{DateTime.UtcNow.Year}{next:D5}";
        var service = new Models.Service
        {
            Id = Guid.NewGuid(),
            Code = ServiceCode,
            Name = request.Name,
            Description = request.Description,
            BasePrice = request.BasePrice,
            EstimatedMinutes =
                request.EstimatedMinutes
        };

        _context.Services.Add(service);

        await _context.SaveChangesAsync();

        return ApiResponse<ServiceResponse>.Ok(
            Map(service),
            "Created successfully");
    }

    public async Task<ApiResponse<List<ServiceResponse>>>
        GetAllAsync(ServiceQuery query)
    {
        var dbQuery = _context.Services
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(
            query.Search))
        {
            dbQuery = dbQuery.Where(x =>
                x.Code.Contains(query.Search) ||
                x.Name.Contains(query.Search));
        }

        var services = await dbQuery
            .Skip((query.Page - 1) *
                query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return ApiResponse<List<ServiceResponse>>
            .Ok(
                services.Select(Map).ToList());
    }

    public async Task<ApiResponse<ServiceResponse>>
        GetByIdAsync(Guid id)
    {
        var service = await _context.Services
            .FirstOrDefaultAsync(x =>
                x.Id == id);

        if (service == null)
        {
            return ApiResponse<ServiceResponse>
                .Fail("Service not found");
        }

        return ApiResponse<ServiceResponse>
            .Ok(Map(service));
    }

    public async Task<ApiResponse<ServiceResponse>>
        UpdateAsync(
            Guid id,
            UpdateServiceRequest request)
    {
        var service = await _context.Services
            .FirstOrDefaultAsync(x =>
                x.Id == id);

        if (service == null)
        {
            return ApiResponse<ServiceResponse>
                .Fail("Service not found");
        }

        service.Code = request.Code;
        service.Name = request.Name;
        service.Description =
            request.Description;
        service.BasePrice =
            request.BasePrice;
        service.EstimatedMinutes =
            request.EstimatedMinutes;

        await _context.SaveChangesAsync();

        return ApiResponse<ServiceResponse>
            .Ok(
                Map(service),
                "Updated successfully");
    }

    public async Task<ApiResponse<string>>
        DeleteAsync(Guid id)
    {
        var service = await _context.Services
            .FirstOrDefaultAsync(x =>
                x.Id == id);

        if (service == null)
        {
            return ApiResponse<string>
                .Fail("Service not found");
        }

        _context.Services.Remove(service);

        await _context.SaveChangesAsync();

        return ApiResponse<string>.Ok(
            "Deleted",
            "Deleted successfully");
    }

    private static ServiceResponse Map(
        Models.Service service)
    {
        return new ServiceResponse
        {
            Id = service.Id,
            Code = service.Code,
            Name = service.Name,
            Description =
                service.Description,
            BasePrice = service.BasePrice,
            EstimatedMinutes =
                service.EstimatedMinutes
        };
    }
}