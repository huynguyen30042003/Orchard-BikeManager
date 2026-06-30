using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ThuVienMedia.Data;
using ThuVienMedia.DTOs.CMThuVienMedia;
using ThuVienMedia.DTOs.FileMedia;
using ThuVienMedia.DTOs.ThuVienMedia;
using ThuVienMedia.Models;
using ThuVienMedia.Responses;
using ThuVienMedia.Services.Interfaces;

namespace ThuVienMedia.Services
{
    public class ThuVienMediaService : IThuVienMediaService
    {
        private readonly ThuVienDbContext _context;

        public ThuVienMediaService(ThuVienDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<ThuVienMediaModel>> CreateAsync(
            CreateThuVienMediaRequest request)
        {
            string? AnhDaiDienPath = null;
            string? ThumbAnhDaiDienPath = null;

            if (request.AnhDaiDien != null)
            {
                var folderPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "uploads",
                    "AnhDaiDien");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var fileName =
                    $"{Guid.NewGuid()}{Path.GetExtension(request.AnhDaiDien.FileName)}";

                var fullPath = Path.Combine(folderPath, fileName);

                using var stream = new FileStream(
                    fullPath,
                    FileMode.Create);

                await request.AnhDaiDien.CopyToAsync(stream);

                AnhDaiDienPath =
                    $"/uploads/AnhDaiDien/{fileName}";
            }
            if (request.ThumbAnhDaiDien != null)
            {
                var folderPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "uploads",
                    "ThumbAnhDaiDien");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var fileName =
                    $"{Guid.NewGuid()}{Path.GetExtension(request.ThumbAnhDaiDien.FileName)}";

                var fullPath = Path.Combine(folderPath, fileName);

                using var stream = new FileStream(
                    fullPath,
                    FileMode.Create);

                await request.ThumbAnhDaiDien.CopyToAsync(stream);

                ThumbAnhDaiDienPath =
                    $"/uploads/ThumbAnhDaiDien/{fileName}";
            }
            var thuVien = new ThuVienMediaModel
            {
                ID = Guid.NewGuid(),
                IDChuyenMucMedia = request.IDChuyenMucMedia,
                TenThuVien = request.TenThuVien,
                UrlAnhDaiDien = AnhDaiDienPath,
                GioiThieu = request.GioiThieu,
                ThuTuSapXep = request.ThuTuSapXep,
                SuDung = request.SuDung,
                LoaiThuVien = request.LoaiThuVien,
                UrlThumbAnhDaiDien = ThumbAnhDaiDienPath,
                CreatedOnDate = DateTime.UtcNow,

                //OwnerCode,CreatedByUserId,ModuleId
            };
            _context.ThuVienMedia.Add(thuVien);
            if (request.Files != null)
            {
                foreach (var item in request.Files)
                {

                    string? filePath = null;
                    string? ThumbPath = null;
                    string? AnhDaiDienVideoPath = null;

                    if (item.File != null)
                    {
                        var folderPath = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot",
                            "uploads",
                            "file");

                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        var fileName =
                            $"{Guid.NewGuid()}{Path.GetExtension(item.File.FileName)}";

                        var fullPath = Path.Combine(folderPath, fileName);

                        using var stream = new FileStream(
                            fullPath,
                            FileMode.Create);

                        await item.File.CopyToAsync(stream);

                        filePath =
                            $"/uploads/file/{fileName}";
                    }
                    if (item.Thumb != null)
                    {
                        var folderPath = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot",
                            "uploads",
                            "thumb");

                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        var ThumbName =
                            $"{Guid.NewGuid()}{Path.GetExtension(item.Thumb.FileName)}";

                        var fullPath = Path.Combine(folderPath, ThumbName);

                        using var stream = new FileStream(
                            fullPath,
                            FileMode.Create);

                        await item.Thumb.CopyToAsync(stream);

                        ThumbPath =
                            $"/uploads/thumb/{ThumbName}";
                    }
                    if (item.AnhDaiDienVideo != null)
                    {
                        var folderPath = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot",
                            "uploads",
                            "anhDaiDienVideo");

                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        var anhDaiDienVideoName =
                            $"{Guid.NewGuid()}{Path.GetExtension(item.AnhDaiDienVideo.FileName)}";

                        var fullPath = Path.Combine(folderPath, anhDaiDienVideoName);

                        using var stream = new FileStream(
                            fullPath,
                            FileMode.Create);

                        await item.AnhDaiDienVideo.CopyToAsync(stream);

                        AnhDaiDienVideoPath =
                            $"/uploads/anhDaiDienVideo/{anhDaiDienVideoName}";
                    }
                    var file = new FileMedia
                    {
                        ID = Guid.NewGuid(),
                        IDThuVienMedia = thuVien.ID,
                        TenFile = item.TenFile,
                        TieuDe = item.TieuDe,
                        MoTa = item.MoTa,
                        TacGia = item.TacGia,
                        ThuTuSapXep = item.ThuTuSapXep,
                        LoaiFile = item.LoaiFile,
                        NguonFile = item.NguonFile,
                        UrlFile = filePath,
                        UrlThumb = ThumbPath,
                        UrlAnhDaiDienVideo = AnhDaiDienVideoPath,
                    };
                    _context.FileMedia.Add(file);
                }
            }
            await _context.SaveChangesAsync();

            return ApiResponse<ThuVienMediaModel>.Ok(
                null,
                "Created successfully");
        }

        public async Task<ApiResponse<ThuVienMediaModel>> UpdateAsync(
                Guid id,
                UpdateThuVienMediaRequest request)
        {
            var thuVien = await _context.ThuVienMedia
                .Include(x => x.Files)
                .FirstOrDefaultAsync(x => x.ID == id);

            if (thuVien == null)
            {
                return ApiResponse<ThuVienMediaModel>.Fail("ThuVien not found");
            }
            thuVien.IDChuyenMucMedia = request.IDChuyenMucMedia;
            thuVien.TenThuVien = request.TenThuVien;
            thuVien.GioiThieu = request.GioiThieu;
            thuVien.MoTa = request.MoTa;
            thuVien.ThuTuSapXep = request.ThuTuSapXep;
            thuVien.SuDung = request.SuDung;
            thuVien.LoaiThuVien = request.LoaiThuVien;
            thuVien.LastModifiedOnDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return ApiResponse<ThuVienMediaModel>.Ok(
                null,
                "Updated successfully");
        }

        public async Task<ApiResponse<object>> UpdateThumbAnhDaiDienAsync(
           Guid id, IFormFile ThumbAnhDaiDien)
        {
            if (ThumbAnhDaiDien == null)
            {
                return ApiResponse<object>.Fail("ThumbAnhDaiDien is null");
            }
            var thuVien = await _context.ThuVienMedia
                .FirstOrDefaultAsync(x => x.ID == id);

            if (thuVien == null)
            {
                return ApiResponse<object>.Fail("ThuVien not found");
            }
            DeleteFile(thuVien.UrlThumbAnhDaiDien);

            string? ThumbAnhDaiDienPath = null;
            if (ThumbAnhDaiDien != null)
            {
                var folderPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "uploads",
                    "ThumbAnhDaiDien");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var fileName =
                    $"{Guid.NewGuid()}{Path.GetExtension(ThumbAnhDaiDien.FileName)}";

                var fullPath = Path.Combine(folderPath, fileName);

                using var stream = new FileStream(
                    fullPath,
                    FileMode.Create);

                await ThumbAnhDaiDien.CopyToAsync(stream);

                ThumbAnhDaiDienPath =
                    $"/uploads/thumbAnhDaiDien/{fileName}";
            }
            if (ThumbAnhDaiDienPath != null)
                thuVien.UrlThumbAnhDaiDien = ThumbAnhDaiDienPath;

            thuVien.LastModifiedOnDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return ApiResponse<object>.Ok(
                "Updated successfully");

        }

        public async Task<ApiResponse<object>> UpdateAnhDaiDienAsync(
           Guid id, IFormFile AnhDaiDien)
        {
            if (AnhDaiDien == null)
            {
                return ApiResponse<object>.Fail("AnhDaiDien is null");
            }
            var thuVien = await _context.ThuVienMedia
                .FirstOrDefaultAsync(x => x.ID == id);

            if (thuVien == null)
            {
                return ApiResponse<object>.Fail("ThuVien not found");
            }
            DeleteFile(thuVien.UrlAnhDaiDien);

            string? AnhDaiDienPath = null;
            if (AnhDaiDien != null)
            {
                var folderPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "uploads",
                    "AnhDaiDien");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var fileName =
                    $"{Guid.NewGuid()}{Path.GetExtension(AnhDaiDien.FileName)}";

                var fullPath = Path.Combine(folderPath, fileName);

                using var stream = new FileStream(
                    fullPath,
                    FileMode.Create);

                await AnhDaiDien.CopyToAsync(stream);

                AnhDaiDienPath =
                    $"/uploads/anhDaiDien/{fileName}";
            }
            if (AnhDaiDienPath != null)
                thuVien.UrlAnhDaiDien = AnhDaiDienPath;

            thuVien.LastModifiedOnDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return ApiResponse<object>.Ok(
                "Updated successfully");
        }

        public async Task<PagedResult<ThuVienMediaResponse>> GetAll(
           ThuVienMediaQuery query)
        {
            const string Collation = "Vietnamese_100_CI_AI";
            var ThuVienMedia = _context.ThuVienMedia
            .Include(x => x.Files)
            .AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.TenThuVien))
            {
                ThuVienMedia = ThuVienMedia.Where(x =>
                x.TenThuVien != null &&
                EF.Functions.Collate(
                        x.TenThuVien,
                        Collation
                    ).Contains(query.TenThuVien));
            }
            if (query.SuDung.HasValue)
            {
                ThuVienMedia = ThuVienMedia.Where(x =>
                    x.SuDung == query.SuDung);
            }
            if (query.IDChuyenMucMedia.HasValue)
            {
                ThuVienMedia = ThuVienMedia.Where(x =>
                    x.IDChuyenMucMedia == query.IDChuyenMucMedia);
            }
            if (query.FromDate.HasValue)
            {
                ThuVienMedia = ThuVienMedia.Where(x =>
                    x.CreatedOnDate >= query.FromDate.Value);
            }

            if (query.ToDate.HasValue)
            {
                var endDate = query.ToDate.Value.Date.AddDays(1);

                ThuVienMedia = ThuVienMedia.Where(x =>
                    x.CreatedOnDate < endDate);
            }
            var totalItems = await ThuVienMedia.CountAsync();
            var result = await ThuVienMedia
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .OrderByDescending(x => x.CreatedOnDate)
            .Select(x => new ThuVienMediaResponse
            {
                ID = x.ID,
                OwnerCode = x.OwnerCode,
                ModuleId = x.ModuleId,
                CreatedByUserId = x.CreatedByUserId,
                LastModifiedByUserId = x.LastModifiedByUserId,
                CreatedOnDate = x.CreatedOnDate,
                LastModifiedOnDate = x.LastModifiedOnDate,
                IDChuyenMucMedia = x.IDChuyenMucMedia,
                TenThuVien = x.TenThuVien,
                MoTa = x.MoTa,
                GioiThieu = x.GioiThieu,
                UrlThumbAnhDaiDien = x.UrlThumbAnhDaiDien,
                UrlAnhDaiDien = x.UrlAnhDaiDien,
                LoaiThuVien = x.LoaiThuVien,
                IDFileMediaList = x.IDFileMediaList,
                ThuTuSapXep = x.ThuTuSapXep,
                SuDung = x.SuDung,
            })
            .ToListAsync();
            return new PagedResult<ThuVienMediaResponse>
            {
                Page = query.Page,
                PageSize = query.PageSize,
                TotalItems = totalItems,

                TotalPages = (int)Math.Ceiling(
                 totalItems / (double)query.PageSize),

                Items = result
            };
        }

        public async Task<ApiResponse<ThuVienMediaResponse>> GetById(
            Guid id)
        {
            var entity = await _context.ThuVienMedia
            .Include(c => c.ChuyenMucMedia)
            .Include(c => c.Files)
            .FirstOrDefaultAsync(c => c.ID == id);

            if (entity == null)
            {
                return ApiResponse<ThuVienMediaResponse>.Fail("ThuVienMedia not found");
            }
            var result = new ThuVienMediaResponse
            {
                ID = entity.ID,
                OwnerCode = entity.OwnerCode,
                ModuleId = entity.ModuleId,
                CreatedByUserId = entity.CreatedByUserId,
                LastModifiedByUserId = entity.LastModifiedByUserId,
                CreatedOnDate = entity.CreatedOnDate,
                LastModifiedOnDate = entity.LastModifiedOnDate,
                IDChuyenMucMedia = entity.IDChuyenMucMedia,
                TenThuVien = entity.TenThuVien,
                MoTa = entity.MoTa,
                GioiThieu = entity.GioiThieu,
                UrlThumbAnhDaiDien = entity.UrlThumbAnhDaiDien,
                UrlAnhDaiDien = entity.UrlAnhDaiDien,
                LoaiThuVien = entity.LoaiThuVien,
                IDFileMediaList = entity.IDFileMediaList,
                ThuTuSapXep = entity.ThuTuSapXep,
                SuDung = entity.SuDung,
                CMThuVienMedia = entity.ChuyenMucMedia != null ? new CMThuVienMediaResponse
                {
                    ID = entity.ChuyenMucMedia.ID,
                    OwnerCode = entity.ChuyenMucMedia.OwnerCode,
                    ModuleId = entity.ChuyenMucMedia.ModuleId,
                    CreatedByUserId = entity.ChuyenMucMedia.CreatedByUserId,
                    LastModifiedByUserId = entity.ChuyenMucMedia.LastModifiedByUserId,
                    CreatedOnDate = entity.ChuyenMucMedia.CreatedOnDate,
                    LastModifiedOnDate = entity.ChuyenMucMedia.LastModifiedOnDate,
                    IDChuyenMucCapCha = entity.ChuyenMucMedia.IDChuyenMucCapCha,
                    TenChuyenMuc = entity.ChuyenMucMedia.TenChuyenMuc,
                    ThuTuSapXep = entity.ChuyenMucMedia.ThuTuSapXep,
                    SuDung = entity.ChuyenMucMedia.SuDung
                } : null,
                Files = entity.Files?
                    .Select(f => new FileMediaResponse
                    {
                        ID = f.ID,
                        OwnerCode = f.OwnerCode,
                        TenFile = f.TenFile,
                        TieuDe = f.TieuDe,
                        MoTa = f.MoTa,
                        TacGia = f.TacGia,
                        ThuTuSapXep = f.ThuTuSapXep,
                        LoaiFile = f.LoaiFile,
                        NguonFile = f.NguonFile,
                        UrlFile = f.UrlFile,
                        UrlThumb = f.UrlThumb,
                        UrlAnhDaiDienVideo = f.UrlAnhDaiDienVideo
                    })
                    .ToList() ?? []
            };
            return ApiResponse<ThuVienMediaResponse>.Ok(result);
        }

        public async Task<ApiResponse<object>> DeleteAsync(
            Guid id)
        {
            var ThuVienMedia = await _context.ThuVienMedia
                .FirstOrDefaultAsync(x => x.ID == id);

            if (ThuVienMedia == null)
            {
                return ApiResponse<object>.Fail(
                "ThuVien not found");
            }

            var medias = await _context.FileMedia
                .Where(x => x.IDThuVienMedia == id)
                .ToListAsync();
            foreach (var media in medias)
            {
                DeleteFile(media.UrlFile);
                DeleteFile(media.UrlThumb);
                DeleteFile(media.UrlAnhDaiDienVideo);
            }
            _context.FileMedia.RemoveRange(medias);

            DeleteFile(ThuVienMedia.UrlAnhDaiDien);
            DeleteFile(ThuVienMedia.UrlThumbAnhDaiDien);
            _context.ThuVienMedia.Remove(ThuVienMedia);

            await _context.SaveChangesAsync();

            return ApiResponse<object>.Ok(
            "Deleted successfully");
        }

        public void DeleteFile(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                Console.WriteLine("DeleteFile null relativePath");


            var fullPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
