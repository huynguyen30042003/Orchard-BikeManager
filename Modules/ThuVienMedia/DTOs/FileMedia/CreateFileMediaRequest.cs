using Microsoft.AspNetCore.Http;

namespace ThuVienMedia.DTOs.FileMedia
{
    public class CreateFileMediaRequest
    {
        public Guid? IDChuyenMucMedia { get; set; }
        public string? TenFile { get; set; }
        public string? TieuDe { get; set; }
        public string? MoTa { get; set; }
        public string? TacGia { get; set; }
        public byte? ThuTuSapXep { get; set; }
        public IFormFile? File { get; set; }
        public IFormFile? Thumb { get; set; }
        public IFormFile? AnhDaiDienVideo { get; set; }
        public byte? LoaiFile { get; set; }
        public byte? NguonFile { get; set; }
    }
}
