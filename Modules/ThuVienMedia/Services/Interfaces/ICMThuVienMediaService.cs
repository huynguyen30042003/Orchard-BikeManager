using ThuVienMedia.DTOs.CMThuVienMedia;
using ThuVienMedia.Models;
using ThuVienMedia.Responses;

namespace ThuVienMedia.Services.Interfaces
{
    public interface ICMThuVienMediaService
    {
        Task<PagedResult<CMThuVienMediaResponse>> GetAll(
            CMThuVienMediaQuery query);

        Task<ApiResponse<CMThuVienMediaResponse>> GetByIdAsync(
            Guid id);

        Task<PagedResult<CMThuVienMediaResponse>> GetAllTree(
            CMThuVienMediaQuery query);

        Task<ApiResponse<CMThuVienMedia>> CreateAsync(
        CreateCMThuVienMediaRequest request);

        Task<ApiResponse<CMThuVienMedia>> UpdateAsync(Guid id,
        UpdateCMThuVienMediaRequest request);

        Task<ApiResponse<object>> DeleteAsync(Guid id);
    }

}
