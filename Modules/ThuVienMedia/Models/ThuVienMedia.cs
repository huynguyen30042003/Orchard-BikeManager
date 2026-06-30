namespace ThuVienMedia.Models
{
    public class ThuVienMediaModel
    {
        public Guid ID { get; set; }
        public string OwnerCode { get; set; } = string.Empty;
        public int? ModuleId { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModifiedByUserId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public DateTime? LastModifiedOnDate { get; set; }
        public Guid? IDChuyenMucMedia { get; set; }
        public string? TenThuVien { get; set; }
        public string? MoTa { get; set; }
        public string? GioiThieu { get; set; }
        public string? UrlThumbAnhDaiDien { get; set; }
        public string? UrlAnhDaiDien { get; set; }
        public byte? LoaiThuVien { get; set; }
        public string? IDFileMediaList { get; set; }
        public byte? ThuTuSapXep { get; set; }
        public bool SuDung { get; set; }
        public CMThuVienMedia? ChuyenMucMedia { get; set; }
        public ICollection<FileMedia> Files { get; set; }
            = new List<FileMedia>();
    }
}
