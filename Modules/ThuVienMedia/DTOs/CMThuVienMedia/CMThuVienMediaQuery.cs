namespace ThuVienMedia.DTOs.CMThuVienMedia
{
    public class CMThuVienMediaQuery
    {
        public string? TenChuyenMuc { get; set; }
        public bool? SuDung { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
