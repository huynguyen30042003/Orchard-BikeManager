using Microsoft.AspNetCore.Http;
using ThuVienMedia.DTOs.ThuVienMedia;
using ThuVienMedia.Models;
using ThuVienMedia.Responses;

namespace ThuVienMedia.Services.Interfaces
{
    public interface IThuVienMediaService
    {
        Task<PagedResult<ThuVienMediaResponse>> GetAll(
            ThuVienMediaQuery query);

        Task<ApiResponse<ThuVienMediaResponse>> GetById(
            Guid id);

        Task<ApiResponse<ThuVienMediaModel>> CreateAsync(
            CreateThuVienMediaRequest request);

        Task<ApiResponse<ThuVienMediaModel>> UpdateAsync(
           Guid id, UpdateThuVienMediaRequest request);

        Task<ApiResponse<object>> UpdateThumbAnhDaiDienAsync(
           Guid id, IFormFile ThumbAnhDaiDien);

        Task<ApiResponse<object>> UpdateAnhDaiDienAsync(
           Guid id, IFormFile AnhDaiDien);

        Task<ApiResponse<object>> DeleteAsync(
            Guid id);
    }
}
