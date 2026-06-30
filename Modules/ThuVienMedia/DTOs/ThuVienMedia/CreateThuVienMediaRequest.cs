using Microsoft.AspNetCore.Http;
using ThuVienMedia.DTOs.FileMedia;

namespace ThuVienMedia.DTOs.ThuVienMedia
{
    public class CreateThuVienMediaRequest
    {
        public Guid? IDChuyenMucMedia { get; set; }
        public string? TenThuVien { get; set; }
        public IFormFile? AnhDaiDien { get; set; }
        public string? GioiThieu { get; set; }
        public byte? ThuTuSapXep { get; set; }
        public bool SuDung { get; set; }
        public byte? LoaiThuVien { get; set; }
        public string? MoTa { get; set; }
        public IFormFile? ThumbAnhDaiDien { get; set; }
        public List<CreateFileMediaRequest>? Files { get; set; } = new List<CreateFileMediaRequest>();
    }
}
