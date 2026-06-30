namespace ThuVienMedia.Models
{
    public class FileMedia
    {
        public Guid ID { get; set; }
        public string OwnerCode { get; set; } = string.Empty;
        public int? ModuleId { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModifiedByUserId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public DateTime? LastModifiedOnDate { get; set; }
        public Guid? IDThuVienMedia { get; set; }
        public string? TenFile { get; set; }
        public string? TieuDe { get; set; }
        public string? MoTa { get; set; }
        public string? TacGia { get; set; }
        public int? IdFile { get; set; }
        public string? UrlFile { get; set; }
        public string? UrlThumb { get; set; }
        public string? UrlAnhDaiDienVideo { get; set; }
        public byte? LoaiFile { get; set; }
        public byte? NguonFile { get; set; }
        public byte? ThuTuSapXep { get; set; }
        public ThuVienMediaModel? ThuVienMedia { get; set; }
    }
}
