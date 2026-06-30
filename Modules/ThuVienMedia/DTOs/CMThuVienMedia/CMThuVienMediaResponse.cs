namespace ThuVienMedia.DTOs.CMThuVienMedia
{
    public class CMThuVienMediaResponse
    {
        public Guid ID { get; set; }
        public string OwnerCode { get; set; } = string.Empty;
        public int? ModuleId { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModifiedByUserId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public DateTime? LastModifiedOnDate { get; set; }
        public Guid? IDChuyenMucCapCha { get; set; }
        public string? TenChuyenMuc { get; set; }
        public byte? ThuTuSapXep { get; set; }
        public bool SuDung { get; set; }
        public List<CMThuVienMediaResponse>? Children { get; set; } = [];
    }
}
