using ThuVienMedia.DTOs.FileMedia;
using ThuVienMedia.DTOs.ThuVienMedia;
using ThuVienMedia.Responses;

namespace ThuVienMedia.Services.Interfaces
{
    public interface IFileMediaService
    {
        Task<PagedResult<FileMediaResponse>> GetPagedAsync(FileMediaQuery query);

        Task<ApiResponse<FileMediaResponse>> GetByIdAsync(Guid id);

        Task<ApiResponse<FileMediaResponse>> CreateAsync(Guid ThuVienId, CreateFileMediaRequest request);

        Task<ApiResponse<FileMediaResponse>> UpdateAsync(
            Guid id,
            UpdateInfoFileMediaRequest request);

        Task<ApiResponse<FileMediaResponse>> DeleteAsync(Guid id);
    }
}
