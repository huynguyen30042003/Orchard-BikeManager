// Services/InstallmentProviderService.cs
using BikeManagerV3.Order.Data;
using BikeManagerV3.Order.DTOs.InstallmentProviders;
using BikeManagerV3.Order.DTOs.Orders;
using BikeManagerV3.Order.Models;
using BikeManagerV3.Order.Responses;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Order.Services;

public class InstallmentProviderService
    : IInstallmentProviderService
{
    private readonly OrderDbContext _context;

    public InstallmentProviderService(
        OrderDbContext context)
    {
        _context = context;
    }

    public async Task<
        ApiResponse<InstallmentProviderResponse>>
        CreateAsync(
            CreateInstallmentProviderRequest request)
    {
        var provider = new InstallmentProvider
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Phone = request.Phone,
            ApiEndpoint =
                request.ApiEndpoint,
            IsActive = request.IsActive
        };

        _context.InstallmentProviders
            .Add(provider);

        await _context.SaveChangesAsync();

        return ApiResponse<
            InstallmentProviderResponse>.Ok(
                Map(provider),
                "Created successfully");
    }

    public async Task<
        PagedResult<InstallmentProviderResponse>>
        GetAllAsync(
            InstallmentProviderQuery query)
    {
        var dbQuery = _context
            .InstallmentProviders
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(
            query.Search))
        {
            dbQuery = dbQuery.Where(x =>
                x.Name.Contains(
                    query.Search) ||
                x.Phone.Contains(
                    query.Search));
        }

        if (query.IsActive.HasValue)
        {
            dbQuery = dbQuery.Where(x =>
                x.IsActive ==
                query.IsActive.Value);
        }
        var totalItems = await dbQuery.CountAsync();
        var InstallmentProvider = await dbQuery
            .Skip((query.Page - 1) *
                query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return new PagedResult<InstallmentProviderResponse>
        {
            Page = query.Page,
            PageSize = query.PageSize,
            TotalItems = totalItems,

            TotalPages = (int)Math.Ceiling(
                totalItems / (double)query.PageSize),

            Items = InstallmentProvider.Select(Map).ToList()
        };
    }

    public async Task<
        ApiResponse<
            InstallmentProviderResponse>>
        GetByIdAsync(Guid id)
    {
        var provider = await _context
            .InstallmentProviders
            .FirstOrDefaultAsync(x =>
                x.Id == id);

        if (provider == null)
        {
            return ApiResponse<
                InstallmentProviderResponse>
                .Fail(
                    "Provider not found");
        }

        return ApiResponse<
            InstallmentProviderResponse>
            .Ok(Map(provider));
    }

    public async Task<
        ApiResponse<
            InstallmentProviderResponse>>
        UpdateAsync(
            Guid id,
            UpdateInstallmentProviderRequest request)
    {
        var provider = await _context
            .InstallmentProviders
            .FirstOrDefaultAsync(x =>
                x.Id == id);

        if (provider == null)
        {
            return ApiResponse<
                InstallmentProviderResponse>
                .Fail(
                    "Provider not found");
        }

        provider.Name = request.Name;
        provider.Phone = request.Phone;
        provider.ApiEndpoint =
            request.ApiEndpoint;
        provider.IsActive =
            request.IsActive;

        await _context.SaveChangesAsync();

        return ApiResponse<
            InstallmentProviderResponse>
            .Ok(
                Map(provider),
                "Updated successfully");
    }

    public async Task<
        ApiResponse<string>>
        DeleteAsync(Guid id)
    {
        var provider = await _context
            .InstallmentProviders
            .FirstOrDefaultAsync(x =>
                x.Id == id);

        if (provider == null)
        {
            return ApiResponse<string>
                .Fail(
                    "Provider not found");
        }

        _context.InstallmentProviders
            .Remove(provider);

        await _context.SaveChangesAsync();

        return ApiResponse<string>.Ok(
            "Deleted",
            "Deleted successfully");
    }

    private static
        InstallmentProviderResponse Map(
            InstallmentProvider provider)
    {
        return new InstallmentProviderResponse
        {
            Id = provider.Id,
            Name = provider.Name,
            Phone = provider.Phone,
            ApiEndpoint =
                provider.ApiEndpoint,
            IsActive =
                provider.IsActive
        };
    }
}