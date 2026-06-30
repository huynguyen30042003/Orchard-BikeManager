namespace ThuVienMedia.DTOs.FileMedia
{
    public class UpdateInfoFileMediaRequest
    {
        public string? TenFile { get; set; }

        public string? TieuDe { get; set; }

        public string? MoTa { get; set; }

        public string? TacGia { get; set; }

        public byte? ThuTuSapXep { get; set; }

        public byte? LoaiFile { get; set; }

        public byte? NguonFile { get; set; }
    }
}
