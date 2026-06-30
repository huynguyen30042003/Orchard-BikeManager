using BikeManagerV3.Customer.DTOs.Customers;
using BikeManagerV3.Customer.Responses;

namespace BikeManagerV3.Customer.Services;

public interface ICustomerService
{
    Task<CustomerResponse> CreateAsync(
        CreateCustomerRequest request);

    Task<PagedResult<CustomerResponse>> GetAllAsync(
        CustomerQuery query);

    Task<CustomerResponse> GetPhoneNumberAsync(
        string PhoneNumber);
    Task<CustomerResponse?> GetByIdAsync(
    Guid id);
    Task<CustomerResponse?> UpdateAsync(
        Guid id,
        UpdateCustomerRequest request);

    Task<bool> DeleteAsync(
        Guid id);
}