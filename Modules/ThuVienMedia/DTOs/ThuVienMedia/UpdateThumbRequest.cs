using Microsoft.AspNetCore.Http;

namespace ThuVienMedia.DTOs.ThuVienMedia
{
    public class UpdateThumbRequest
    {
        public IFormFile Thumb { get; set; } = null!;
    }
}
