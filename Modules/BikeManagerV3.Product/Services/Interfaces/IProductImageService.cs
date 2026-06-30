using BikeManagerV3.Product.DTOs.ProductImage;
using BikeManagerV3.Product.Responses;

namespace BikeManagerV3.Product.Services.Interfaces;

public interface IProductImageService
{
    Task<ApiResponse<object>> GetAll(
        Guid? productId,
        bool? isThumbnail);

    Task<ApiResponse<object>> GetById(Guid id);

    Task<ApiResponse<object>> Create(
        CreateProductImageRequest request);

    Task<ApiResponse<object>> Update(
        Guid id,
        UpdateProductImageRequest request);

    Task<ApiResponse<object>> Delete(Guid id);
}