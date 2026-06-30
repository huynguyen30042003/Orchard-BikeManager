using BikeManagerV3.Product.DTOs.ProductVariant;
using BikeManagerV3.Product.Responses;

namespace BikeManagerV3.Product.Services.Interfaces;

public interface IProductVariantService
{
    Task<PagedResult<ProductVariantResponse>> GetAll(
        ProductVariantQuery query);

    Task<ApiResponse<object>> GetById(
        Guid id);
    Task<ApiResponse<object>> GetByProductId(
    Guid productId);

    Task<ApiResponse<object>> Create(
        CreateProductVariantRequest request);

    Task<ApiResponse<object>> Update(
        Guid id,
        UpdateProductVariantRequest request);

    Task<ApiResponse<object>> Delete(
        Guid id);

    Task<ApiResponse<object>> GetDashboardProduct();

}