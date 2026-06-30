namespace ThuVienMedia.DTOs.CMThuVienMedia
{
    public class UpdateCMThuVienMediaRequest
    {
        public Guid? IDChuyenMucCapCha { get; set; }
        public string? TenChuyenMuc { get; set; }
        public byte? ThuTuSapXep { get; set; }
        public bool SuDung { get; set; }
    }
}
