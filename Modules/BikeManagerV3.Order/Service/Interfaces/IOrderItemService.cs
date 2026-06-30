// Services/IOrderItemService.cs
using BikeManagerV3.Order.DTOs.OrderItems;
using BikeManagerV3.Order.Responses;

namespace BikeManagerV3.Order.Services;

public interface IOrderItemService
{
    Task<ApiResponse<OrderItemResponse>>
        CreateAsync(
            CreateOrderItemRequest request);

    Task<PagedResult<OrderItemResponse>>
        GetAllAsync(
            OrderItemQuery query);

    Task<ApiResponse<OrderItemResponse>>
        GetByIdAsync(
            Guid id);

    Task<ApiResponse<OrderItemResponse>>
        UpdateAsync(
            Guid id,
            UpdateOrderItemRequest request);

    Task<ApiResponse<string>>
        DeleteAsync(
            Guid id);
}