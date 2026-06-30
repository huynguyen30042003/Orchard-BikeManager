using BikeManagerV3.Order.DTOs.Orders;
using BikeManagerV3.Order.DTOs.Sales;
using BikeManagerV3.Order.Responses;

namespace BikeManagerV3.Order.Service.Interfaces;

public interface ISalesWorkflowService
{
    Task<ApiResponse<OrderResponse>> CreateSaleOrderAsync(
        CreateSaleOrderRequest request);

    Task<ApiResponse<OrderResponse>> CreateInstallmentOrderAsync(
        CreateInstallmentOrderRequest request);

    Task CancelOrderAsync(Guid orderId);

    Task ReturnOrderAsync(Guid orderId);
}