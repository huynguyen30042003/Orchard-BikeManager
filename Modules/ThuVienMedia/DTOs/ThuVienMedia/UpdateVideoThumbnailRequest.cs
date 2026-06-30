using Microsoft.AspNetCore.Http;

namespace ThuVienMedia.DTOs.ThuVienMedia
{
    public class UpdateVideoThumbnailRequest
    {
        public IFormFile AnhDaiDienVideo { get; set; } = null!;
    }
}
