using BikeManagerV3.Suppliers.DTOs.PurchaseOrder;
using BikeManagerV3.Suppliers.Responses;

namespace BikeManagerV3.Suppliers.Service.Interface
{
    public interface IPurchaseOrderService
    {
        Task<ApiResponse<PurchaseOrderResponse>> CreateAsync(
            CreatePurchaseOrderRequest request, string currentUserId);

        Task<ApiResponse<PurchaseOrderResponse>> GetByIdAsync(Guid id);

        Task<PagedResult<PurchaseOrderResponse>> GetPagedAsync(
            PurchaseOrderQuery query);

        Task<ApiResponse<object>> ApproveAsync(Guid id);

        Task<ApiResponse<object>> CancelAsync(Guid id);

        Task<ApiResponse<object>> ReceiveAsync(
            Guid id,
            ReceivePurchaseOrderRequest request
            );
        Task<ApiResponse<object>> ApplyAsync(Guid id);
    }
}
