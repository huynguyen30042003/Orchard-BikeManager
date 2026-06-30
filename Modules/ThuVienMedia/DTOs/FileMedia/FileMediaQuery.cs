namespace ThuVienMedia.DTOs.FileMedia
{
    public class FileMediaQuery
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Guid? IDThuVienMedia { get; set; } = null;
        public string? Keyword { get; set; } = null;
    }
}
