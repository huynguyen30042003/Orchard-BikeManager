using BikeManagerV3.Suppliers.DTOs.Supplier;
using BikeManagerV3.Suppliers.Responses;

namespace BikeManagerV3.Suppliers.Service.Interface
{
    public interface ISupplierService
    {
        Task<ApiResponse<SupplierResponse>> CreateAsync(CreateSupplierRequest request);

        Task<ApiResponse<SupplierResponse>> UpdateAsync(
            Guid id,
            UpdateSupplierRequest request);

        Task DeleteAsync(Guid id);

        Task<ApiResponse<SupplierResponse>> GetByIdAsync(Guid id);

        Task<PagedResult<SupplierResponse>> GetPagedAsync(
            SupplierQuery query);
    }
}
