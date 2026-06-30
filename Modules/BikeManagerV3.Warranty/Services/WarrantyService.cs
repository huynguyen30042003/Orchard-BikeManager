using BikeManagerV3.Warranty.Data;
using BikeManagerV3.Warranty.DTOs.Warranties;
using BikeManagerV3.Warranty.Models;
using BikeManagerV3.Warranty.Responses;
using BikeManagerV3.Warranty.Services.Interfaces;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Warranty.Services;

public class WarrantyService : IWarrantyService
{
    private readonly WarrantyDbContext _context;

    public WarrantyService(WarrantyDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<WarrantyDto>> GetAllAsync(
        WarrantyQuery request)
    {
        var query = _context.Warranties
            .AsQueryable();

        // FILTER CUSTOMER
        if (request.CustomerId.HasValue)
        {
            query = query.Where(x =>
                x.CustomerId == request.CustomerId.Value);
        }

        // FILTER ORDER
        if (request.OrderId.HasValue)
        {
            query = query.Where(x =>
                x.OrderId == request.OrderId.Value);
        }

        // FILTER SERIAL
        if (request.SerialNumberId.HasValue)
        {
            query = query.Where(x =>
                x.SerialNumberId == request.SerialNumberId.Value);
        }

        // FILTER STATUS
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            query = query.Where(x =>
                x.Status == request.Status);
        }

        // FILTER START DATE
        if (request.StartDate.HasValue)
        {
            var startDateOnly =
                DateOnly.FromDateTime(request.StartDate.Value);

            query = query.Where(x =>
                x.StartDate >= startDateOnly);
        }

        // FILTER END DATE
        if (request.EndDate.HasValue)
        {
            var endDateOnly =
                DateOnly.FromDateTime(request.EndDate.Value);

            query = query.Where(x =>
                x.EndDate <= endDateOnly);
        }

        var totalItems = await query.CountAsync();

        var warranties = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return new PagedResult<WarrantyDto>
        {
            Page = request.Page,
            PageSize = request.PageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(
                totalItems / (double)request.PageSize),
            Items = warranties.Select(Map).ToList()
        };
    }

    public async Task<WarrantyDto?> GetByIdAsync(Guid id)
    {
        var warranty = await _context.Warranties.FindAsync(id);

        if (warranty == null)
            return null;

        return new WarrantyDto
        {
            Id = warranty.Id,
            SerialNumberId = warranty.SerialNumberId,
            CustomerId = warranty.CustomerId,
            OrderId = warranty.OrderId,
            StartDate = warranty.StartDate,
            EndDate = warranty.EndDate,
            Status = warranty.Status
        };
    }

    public async Task<WarrantyDto> CreateAsync(CreateWarrantyDto dto)
    {
        if (dto.EndDate <= dto.StartDate)
            throw new Exception("EndDate must be greater than StartDate");

        var warranty = new WarrantyModel
        {
            Id = Guid.NewGuid(),
            SerialNumberId = dto.SerialNumberId,
            CustomerId = dto.CustomerId,
            OrderId = dto.OrderId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Status = dto.Status
        };

        _context.Warranties.Add(warranty);

        await _context.SaveChangesAsync();

        return new WarrantyDto
        {
            Id = warranty.Id,
            SerialNumberId = warranty.SerialNumberId,
            CustomerId = warranty.CustomerId,
            OrderId = warranty.OrderId,
            StartDate = warranty.StartDate,
            EndDate = warranty.EndDate,
            Status = warranty.Status
        };
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateWarrantyDto dto)
    {
        var warranty = await _context.Warranties.FindAsync(id);

        if (warranty == null)
            return false;

        if (dto.EndDate <= dto.StartDate)
            throw new Exception("EndDate must be greater than StartDate");

        warranty.StartDate = dto.StartDate;
        warranty.EndDate = dto.EndDate;
        warranty.Status = dto.Status;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var warranty = await _context.Warranties.FindAsync(id);

        if (warranty == null)
            return false;

        _context.Warranties.Remove(warranty);

        await _context.SaveChangesAsync();

        return true;
    }

    private static WarrantyDto Map(
      WarrantyModel warranty)
    {
        return new WarrantyDto
        {
            Id = warranty.Id,
            SerialNumberId = warranty.SerialNumberId,
            CustomerId = warranty.CustomerId,
            OrderId = warranty.OrderId,
            StartDate = warranty.StartDate,
            EndDate = warranty.EndDate,
            Status = warranty.Status
        };
    }
}