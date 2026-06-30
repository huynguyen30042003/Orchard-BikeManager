using Microsoft.EntityFrameworkCore;
using ThuVienMedia.Data;
using ThuVienMedia.DTOs.CMThuVienMedia;
using ThuVienMedia.Models;
using ThuVienMedia.Responses;
using ThuVienMedia.Services.Interfaces;

namespace ThuVienMedia.Services;

public class CMThuVienMediaService : ICMThuVienMediaService
{
    private readonly ThuVienDbContext _context;

    public CMThuVienMediaService(ThuVienDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<CMThuVienMediaResponse>> GetByIdAsync(Guid id)
    {
        var entity = await _context.CMThuVienMedia
            .Include(c => c.ChuyenMucCapCha)
            .FirstOrDefaultAsync(c => c.ID == id);
        if (entity == null)
        {
            return ApiResponse<CMThuVienMediaResponse>.Fail("Entity not found");
        }
        var result = new CMThuVienMediaResponse
        {
            ID = entity.ID,
            OwnerCode = entity.OwnerCode,
            ModuleId = entity.ModuleId,
            CreatedByUserId = entity.CreatedByUserId,
            LastModifiedByUserId = entity.LastModifiedByUserId,
            CreatedOnDate = entity.CreatedOnDate,
            LastModifiedOnDate = entity.LastModifiedOnDate,
            IDChuyenMucCapCha = entity.IDChuyenMucCapCha,
            TenChuyenMuc = entity.TenChuyenMuc,
            ThuTuSapXep = entity.ThuTuSapXep,
            SuDung = entity.SuDung,
        };

        return ApiResponse<CMThuVienMediaResponse>.Ok(result);
    }

    public async Task<PagedResult<CMThuVienMediaResponse>> GetAll(
       CMThuVienMediaQuery query)
    {
        const string Collation = "Vietnamese_100_CI_AI";
        var CMThuVienMedia = _context.CMThuVienMedia
            .Include(x => x.ChuyenMucCapCha)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.TenChuyenMuc))
        {
            CMThuVienMedia = CMThuVienMedia.Where(x =>
            x.TenChuyenMuc != null &&
            EF.Functions.Collate(
                    x.TenChuyenMuc,
                    Collation
                ).Contains(query.TenChuyenMuc));
        }
        if (query.SuDung.HasValue)
        {
            CMThuVienMedia = CMThuVienMedia.Where(x =>
                x.SuDung == query.SuDung);
        }
        if (query.FromDate.HasValue)
        {
            CMThuVienMedia = CMThuVienMedia.Where(x =>
                x.CreatedOnDate >= query.FromDate.Value);
        }

        if (query.ToDate.HasValue)
        {
            var endDate = query.ToDate.Value.Date.AddDays(1);

            CMThuVienMedia = CMThuVienMedia.Where(x =>
                x.CreatedOnDate < endDate);
        }
        var totalItems = await CMThuVienMedia.CountAsync();
        var result = await CMThuVienMedia
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .OrderByDescending(x => x.CreatedOnDate)
            .Select(x => new CMThuVienMediaResponse
            {
                ID = x.ID,
                OwnerCode = x.OwnerCode,
                ModuleId = x.ModuleId,
                CreatedByUserId = x.CreatedByUserId,
                LastModifiedByUserId = x.LastModifiedByUserId,
                CreatedOnDate = x.CreatedOnDate,
                LastModifiedOnDate = x.LastModifiedOnDate,
                IDChuyenMucCapCha = x.IDChuyenMucCapCha,
                TenChuyenMuc = x.TenChuyenMuc,
                ThuTuSapXep = x.ThuTuSapXep,
                SuDung = x.SuDung,
            })
            .ToListAsync();
        return new PagedResult<CMThuVienMediaResponse>
        {
            Page = query.Page,
            PageSize = query.PageSize,
            TotalItems = totalItems,

            TotalPages = (int)Math.Ceiling(
                 totalItems / (double)query.PageSize),

            Items = result
        };
    }

    public async Task<PagedResult<CMThuVienMediaResponse>> GetAllTree(
        CMThuVienMediaQuery query)
    {
        const string Collation = "Vietnamese_100_CI_AI";

        var cmThuVienMediaQuery = _context.CMThuVienMedia
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.TenChuyenMuc))
        {
            cmThuVienMediaQuery = cmThuVienMediaQuery.Where(x =>
                x.TenChuyenMuc != null &&
                EF.Functions.Collate(
                    x.TenChuyenMuc,
                    Collation)
                .Contains(query.TenChuyenMuc));
        }

        if (query.SuDung.HasValue)
        {
            cmThuVienMediaQuery = cmThuVienMediaQuery.Where(x =>
                x.SuDung == query.SuDung);
        }

        if (query.FromDate.HasValue)
        {
            cmThuVienMediaQuery = cmThuVienMediaQuery.Where(x =>
                x.CreatedOnDate >= query.FromDate.Value);
        }

        if (query.ToDate.HasValue)
        {
            var endDate = query.ToDate.Value.Date.AddDays(1);

            cmThuVienMediaQuery = cmThuVienMediaQuery.Where(x =>
                x.CreatedOnDate < endDate);
        }

        var totalItems = await cmThuVienMediaQuery.CountAsync();

        var items = await cmThuVienMediaQuery
            .OrderBy(x => x.ThuTuSapXep)
            .ThenBy(x => x.TenChuyenMuc)
            .Select(x => new CMThuVienMediaResponse
            {
                ID = x.ID,
                OwnerCode = x.OwnerCode,
                ModuleId = x.ModuleId,
                CreatedByUserId = x.CreatedByUserId,
                LastModifiedByUserId = x.LastModifiedByUserId,
                CreatedOnDate = x.CreatedOnDate,
                LastModifiedOnDate = x.LastModifiedOnDate,
                IDChuyenMucCapCha = x.IDChuyenMucCapCha,
                TenChuyenMuc = x.TenChuyenMuc,
                ThuTuSapXep = x.ThuTuSapXep,
                SuDung = x.SuDung
            })
            .ToListAsync();

        var tree = BuildTree(items);

        return new PagedResult<CMThuVienMediaResponse>
        {
            Page = query.Page,
            PageSize = query.PageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(
                totalItems / (double)query.PageSize),
            Items = tree
        };
    }

    public async Task<ApiResponse<CMThuVienMedia>> CreateAsync(
        CreateCMThuVienMediaRequest request)
    {
        var CMThuVienMedia = new CMThuVienMedia
        {
            ID = Guid.NewGuid(),
            IDChuyenMucCapCha = request.IDChuyenMucCapCha,
            TenChuyenMuc = request.TenChuyenMuc,
            ThuTuSapXep = request.ThuTuSapXep,
            SuDung = request.SuDung,
            CreatedOnDate = DateTime.UtcNow,

            //OwnerCode,CreatedByUserId,ModuleId
        };

        _context.CMThuVienMedia.Add(
            CMThuVienMedia);
        await _context.SaveChangesAsync();

        return ApiResponse<CMThuVienMedia>.Ok(
            CMThuVienMedia,
            "Created successfully");
    }

    public async Task<ApiResponse<CMThuVienMedia>> UpdateAsync(
    Guid id, UpdateCMThuVienMediaRequest request)
    {
        var variant = await _context.CMThuVienMedia
           .FirstOrDefaultAsync(x =>
               x.ID == id);
        if (variant == null)
        {
            return ApiResponse<CMThuVienMedia>.Fail(
                "CMThuVien not found");
        }
        variant.IDChuyenMucCapCha = request.IDChuyenMucCapCha;
        variant.TenChuyenMuc = request.TenChuyenMuc;
        variant.ThuTuSapXep = request.ThuTuSapXep;
        variant.SuDung = request.SuDung;
        variant.LastModifiedOnDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return ApiResponse<CMThuVienMedia>.Ok(
            variant,
            "Created successfully");
    }

    public async Task<ApiResponse<object>> DeleteAsync(Guid id)
    {
        var variant = await _context.CMThuVienMedia
       .FirstOrDefaultAsync(x =>
           x.ID == id);
        if (variant == null)
        {
            return ApiResponse<object>.Fail(
                "CMThuVien not found");
        }
        _context.CMThuVienMedia.Remove(
            variant);
        await _context.SaveChangesAsync();
        return ApiResponse<object>.Ok(
            "Deleted successfully");
    }
    private static List<CMThuVienMediaResponse> BuildTree(
    List<CMThuVienMediaResponse> items)
    {
        var lookup = items.ToDictionary(x => x.ID);

        var roots = new List<CMThuVienMediaResponse>();

        foreach (var item in items)
        {
            if (item.IDChuyenMucCapCha.HasValue
                && lookup.TryGetValue(
                    item.IDChuyenMucCapCha.Value,
                    out var parent))
            {
                parent.Children.Add(item);
            }
            else
            {
                roots.Add(item);
            }
        }

        return roots;
    }
}
