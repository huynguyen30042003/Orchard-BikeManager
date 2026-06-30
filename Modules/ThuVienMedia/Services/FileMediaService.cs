using Microsoft.EntityFrameworkCore;
using ThuVienMedia.Data;
using ThuVienMedia.DTOs.FileMedia;
using ThuVienMedia.Models;
using ThuVienMedia.Responses;
using ThuVienMedia.Services.Interfaces;

namespace ThuVienMedia.Services
{
    public class FileMediaService : IFileMediaService
    {
        private readonly ThuVienDbContext _context;

        public FileMediaService(ThuVienDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<FileMediaResponse>> GetPagedAsync(FileMediaQuery query)
        {
            var fileMedia = _context.FileMedia
            .AsQueryable();
            if (query.IDThuVienMedia.HasValue)
            {
                fileMedia = fileMedia.Where(x =>
                    x.IDThuVienMedia == query.IDThuVienMedia);
            }
            var totalItems = await fileMedia.CountAsync();

            var result = await fileMedia
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .OrderByDescending(x => x.CreatedOnDate)
            .Select(x => new FileMediaResponse
            {
                ID = x.ID,
                OwnerCode = x.OwnerCode,
                ModuleId = x.ModuleId,
                CreatedByUserId = x.CreatedByUserId,
                LastModifiedByUserId = x.LastModifiedByUserId,
                CreatedOnDate = x.CreatedOnDate,
                LastModifiedOnDate = x.LastModifiedOnDate,
                IDThuVienMedia = x.IDThuVienMedia,
                TenFile = x.TenFile,
                TieuDe = x.TieuDe,
                MoTa = x.MoTa,
                TacGia = x.TacGia,
                IdFile = x.IdFile,
                UrlFile = x.UrlFile,
                UrlThumb = x.UrlThumb,
                UrlAnhDaiDienVideo = x.UrlAnhDaiDienVideo,
                LoaiFile = x.LoaiFile,
                ThuTuSapXep = x.ThuTuSapXep,
                NguonFile = x.NguonFile,
            })
            .ToListAsync() ?? [];

            return new PagedResult<FileMediaResponse>
            {
                Page = query.Page,
                PageSize = query.PageSize,
                TotalItems = totalItems,

                TotalPages = (int)Math.Ceiling(
                 totalItems / (double)query.PageSize),

                Items = result
            };
        }

        public async Task<ApiResponse<FileMediaResponse>> GetByIdAsync(Guid id)
        {
            var fileMedia = await _context.FileMedia
                .FindAsync(id);
            if (fileMedia == null)
            {
                return ApiResponse<FileMediaResponse>.Fail("File media not found.");
            }
            var result = ToResponse(fileMedia);
            return ApiResponse<FileMediaResponse>.Ok(result,
                "File media retrieved successfully.");
        }

        public async Task<ApiResponse<FileMediaResponse>> CreateAsync(Guid ThuVienId, CreateFileMediaRequest request)
        {
            var entity = await _context.ThuVienMedia
                .FirstOrDefaultAsync(c => c.ID == ThuVienId);
            if (entity == null)
            {
                return ApiResponse<FileMediaResponse>.Fail("ThuVienMedia not found");
            }
            string? filePath = null;
            string? thumbPath = null;
            string? videoPath = null;
            if (request.File != null)
            {
                var folder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "uploads",
                    "file");

                Directory.CreateDirectory(folder);

                var fileName =
                    $"{Guid.NewGuid()}{Path.GetExtension(request.File.FileName)}";

                var fullPath = Path.Combine(folder, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);

                await request.File.CopyToAsync(stream);

                filePath = $"/uploads/file/{fileName}";
            }

            if (request.Thumb != null)
            {
                var folder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "uploads",
                    "thumb");

                Directory.CreateDirectory(folder);

                var fileName =
                    $"{Guid.NewGuid()}{Path.GetExtension(request.Thumb.FileName)}";

                var fullPath = Path.Combine(folder, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);

                await request.Thumb.CopyToAsync(stream);

                thumbPath = $"/uploads/thumb/{fileName}";
            }

            if (request.AnhDaiDienVideo != null)
            {
                var folder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "uploads",
                    "anhDaiDienVideo");

                Directory.CreateDirectory(folder);

                var fileName =
                    $"{Guid.NewGuid()}{Path.GetExtension(request.AnhDaiDienVideo.FileName)}";

                var fullPath = Path.Combine(folder, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);

                await request.AnhDaiDienVideo.CopyToAsync(stream);

                videoPath = $"/uploads/anhDaiDienVideo/{fileName}";
            }
            var file = new FileMedia
            {
                ID = Guid.NewGuid(),
                IDThuVienMedia = ThuVienId,
                TenFile = request.TenFile,
                TieuDe = request.TieuDe,
                MoTa = request.MoTa,
                TacGia = request.TacGia,
                ThuTuSapXep = request.ThuTuSapXep,
                LoaiFile = request.LoaiFile,
                NguonFile = request.NguonFile,
                UrlFile = filePath,
                UrlThumb = thumbPath,
                UrlAnhDaiDienVideo = videoPath,
            };
            _context.FileMedia.Add(file);

            await _context.SaveChangesAsync();
            var result = ToResponse(file);

            return ApiResponse<FileMediaResponse>.Ok(
                result,
                "Updated successfully");
        }

        public async Task<ApiResponse<FileMediaResponse>> UpdateAsync(
            Guid id,
            UpdateInfoFileMediaRequest request)
        {
            var fileMedia = await _context.FileMedia
                .FindAsync(id);
            if (fileMedia == null)
            {
                return ApiResponse<FileMediaResponse>.Fail("File media not found.");
            }
            fileMedia.TenFile = request.TenFile;
            fileMedia.TieuDe = request.TieuDe;
            fileMedia.MoTa = request.MoTa;
            fileMedia.TacGia = request.TacGia;
            fileMedia.ThuTuSapXep = request.ThuTuSapXep;
            fileMedia.LoaiFile = request.LoaiFile;
            fileMedia.NguonFile = request.NguonFile;
            await _context.SaveChangesAsync();
            var result = ToResponse(fileMedia);

            return ApiResponse<FileMediaResponse>.Ok(
                result,
                "Updated successfully");
        }

        public async Task<ApiResponse<FileMediaResponse>> DeleteAsync(Guid id)
        {
            var fileMedia = await _context.FileMedia
                .FindAsync(id);
            if (fileMedia != null)
            {
                DeleteFile(fileMedia.UrlFile);
                DeleteFile(fileMedia.UrlThumb);
                DeleteFile(fileMedia.UrlAnhDaiDienVideo);
                _context.FileMedia.Remove(fileMedia);
                await _context.SaveChangesAsync();
                var result = ToResponse(fileMedia);

                return ApiResponse<FileMediaResponse>.Ok(
                    result,
                    "Deleted successfully");
            }
            return new ApiResponse<FileMediaResponse>
            {
                Success = false,
            };
        }

        public void DeleteFile(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                Console.WriteLine("DeleteFile null relativePath"); ;

            var fullPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        private static FileMediaResponse ToResponse(FileMedia entity)
        {
            return new FileMediaResponse
            {
                ID = entity.ID,
                IDThuVienMedia = entity.IDThuVienMedia,
                TenFile = entity.TenFile,
                TieuDe = entity.TieuDe,
                MoTa = entity.MoTa,
                TacGia = entity.TacGia,
                IdFile = entity.IdFile,
                UrlFile = entity.UrlFile,
                UrlThumb = entity.UrlThumb,
                UrlAnhDaiDienVideo = entity.UrlAnhDaiDienVideo,
                LoaiFile = entity.LoaiFile,
                NguonFile = entity.NguonFile,
                ThuTuSapXep = entity.ThuTuSapXep,
                CreatedByUserId = entity.CreatedByUserId,
                LastModifiedByUserId = entity.LastModifiedByUserId,
                CreatedOnDate = entity.CreatedOnDate,
                LastModifiedOnDate = entity.LastModifiedOnDate
            };
        }
    }
}
