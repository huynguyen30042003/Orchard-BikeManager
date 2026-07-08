// Services/ICustomerStatisticService.cs
using BikeManagerV3.Customer.DTOs.CustomerStatistics;
using BikeManagerV3.Customer.Responses;

namespace BikeManagerV3.Customer.Services;

public interface ICustomerStatisticService
{
    Task<CustomerStatisticResponse> CreateAsync(
        CreateCustomerStatisticRequest request);

    Task<PagedResult<CustomerStatisticResponse>> GetAllAsync(
        CustomerStatisticQuery query);

    Task<CustomerStatisticResponse?> GetByIdAsync(
        Guid customerId);

    Task<CustomerStatisticResponse?> UpdateAsync(
        Guid customerId,
        UpdateCustomerStatisticRequest request);

    Task<bool> DeleteAsync(
        Guid customerId);
}