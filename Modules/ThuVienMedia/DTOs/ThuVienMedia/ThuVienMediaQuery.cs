namespace ThuVienMedia.DTOs.ThuVienMedia
{
    public class ThuVienMediaQuery
    {
        public Guid? IDChuyenMucMedia { get; set; }
        public bool? SuDung { get; set; }
        public string? TenThuVien { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
