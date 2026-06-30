namespace ThuVienMedia.DTOs.ThuVienMedia
{
    public class UpdateThuVienMediaRequest
    {
        public Guid? IDChuyenMucMedia { get; set; }
        public string? TenThuVien { get; set; }
        public string? GioiThieu { get; set; }
        public byte? ThuTuSapXep { get; set; }
        public string? MoTa { get; set; }
        public bool SuDung { get; set; }
        public byte? LoaiThuVien { get; set; }
    }
}
