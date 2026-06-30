using BikeManagerV3.Warranty.Data;
using BikeManagerV3.Warranty.DTOs.WarrantyClaims;
using BikeManagerV3.Warranty.Models;
using BikeManagerV3.Warranty.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Warranty.Services;

public class WarrantyClaimService : IWarrantyClaimService
{
    private readonly WarrantyDbContext _context;

    public WarrantyClaimService(WarrantyDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WarrantyClaimDto>> GetAllAsync(
       WarrantyClaimQuery request)
    {
        var query = _context.WarrantyClaims
            .AsQueryable();

        // FILTER WARRANTY
        if (request.WarrantyId.HasValue)
        {
            query = query.Where(x =>
                x.WarrantyId == request.WarrantyId.Value);
        }

        // FILTER REPAIR ORDER
        if (request.RepairOrderId.HasValue)
        {
            query = query.Where(x =>
                x.RepairOrderId == request.RepairOrderId.Value);
        }

        // FILTER STATUS
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            query = query.Where(x =>
                x.Status == request.Status);
        }

        return await query
            .Select(x => new WarrantyClaimDto
            {
                Id = x.Id,
                WarrantyId = x.WarrantyId,
                RepairOrderId = x.RepairOrderId,
                IssueDescription = x.IssueDescription,
                Resolution = x.Resolution,
                Status = x.Status
            })
            .ToListAsync();
    }

    public async Task<WarrantyClaimDto?> GetByIdAsync(Guid id)
    {
        var claim = await _context.WarrantyClaims.FindAsync(id);

        if (claim == null)
            return null;

        return new WarrantyClaimDto
        {
            Id = claim.Id,
            WarrantyId = claim.WarrantyId,
            RepairOrderId = claim.RepairOrderId,
            IssueDescription = claim.IssueDescription,
            Resolution = claim.Resolution,
            Status = claim.Status
        };
    }

    public async Task<WarrantyClaimDto> CreateAsync(CreateWarrantyClaimDto dto)
    {
        var warranty = await _context.Warranties
            .FirstOrDefaultAsync(x => x.Id == dto.WarrantyId);

        if (warranty == null)
            throw new Exception("Warranty not found");

        if (warranty.EndDate < DateOnly.FromDateTime(DateTime.UtcNow))
            throw new Exception("Warranty expired");

        var claim = new WarrantyClaim
        {
            Id = Guid.NewGuid(),
            WarrantyId = dto.WarrantyId,
            RepairOrderId = dto.RepairOrderId,
            IssueDescription = dto.IssueDescription,
            Resolution = dto.Resolution,
            Status = dto.Status
        };

        _context.WarrantyClaims.Add(claim);

        await _context.SaveChangesAsync();

        return new WarrantyClaimDto
        {
            Id = claim.Id,
            WarrantyId = claim.WarrantyId,
            RepairOrderId = claim.RepairOrderId,
            IssueDescription = claim.IssueDescription,
            Resolution = claim.Resolution,
            Status = claim.Status
        };
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateWarrantyClaimDto dto)
    {
        var claim = await _context.WarrantyClaims.FindAsync(id);

        if (claim == null)
            return false;

        claim.IssueDescription = dto.IssueDescription;
        claim.Resolution = dto.Resolution;
        claim.Status = dto.Status;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var claim = await _context.WarrantyClaims.FindAsync(id);

        if (claim == null)
            return false;

        _context.WarrantyClaims.Remove(claim);

        await _context.SaveChangesAsync();

        return true;
    }
}