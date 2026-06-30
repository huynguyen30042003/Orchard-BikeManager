using BikeManagerV3.Order.DTOs.Orders;
using BikeManagerV3.Order.Responses;

namespace BikeManagerV3.Order.Services;

public interface IOrderService
{
    Task<ApiResponse<OrderResponse>>
        CreateAsync(
            CreateOrderRequest request);

    Task<PagedResult<OrderResponse>>
        GetAllAsync(
            OrderQuery query);

    Task<ApiResponse<OrderResponse>>
        GetByIdAsync(
            Guid id);

    Task<ApiResponse<OrderResponse>>
        UpdateAsync(
            Guid id,
            UpdateOrderRequest request);

    Task<ApiResponse<string>>
        DeleteAsync(
            Guid id);
}