using BikeManagerV3.Customer.DTOs.Customers;
using BikeManagerV3.Customer.Responses;
using BikeManagerV3.Customer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneNumbers;

namespace BikeManagerV3.Customer.Controllers;

[Authorize(AuthenticationSchemes = "OpenIddict.Validation.AspNetCore")]
[ApiController]
[IgnoreAntiforgeryToken]
[Route("api/v1/customers")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _service;

    public CustomersController(
        ICustomerService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateCustomerRequest request)
    {
        var result = await _service.CreateAsync(request);

        return Ok(ApiResponse<CustomerResponse>.Ok(
            result,
            "Created successfully"));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] CustomerQuery query)
    {
        var result = await _service.GetAllAsync(query);

        return Ok(
          ApiResponse<PagedResult<CustomerResponse>>
          .Ok(result)
      );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        Guid id)
    {
        var result = await _service.GetByIdAsync(id);

        if (result == null)
        {
            return NotFound(
                ApiResponse<string>.Fail("Customer not found"));
        }

        return Ok(ApiResponse<CustomerResponse>.Ok(
            result));
    }

    [HttpGet("phoneNumber/{phoneNumber}")]
    public async Task<IActionResult> GetPhoneNumber(
        string PhoneNumber)
    {
        var result = await _service.GetPhoneNumberAsync(PhoneNumber);

        if (result == null)
        {
            return NotFound(
                ApiResponse<string>.Fail("Customer not found"));
        }

        return Ok(ApiResponse<CustomerResponse>.Ok(
            result));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateCustomerRequest request)
    {
        var result = await _service.UpdateAsync(id, request);

        if (result == null)
        {
            return NotFound(
                ApiResponse<string>.Fail("Customer not found"));
        }

        return Ok(ApiResponse<CustomerResponse>.Ok(
            result,
            "Updated successfully"));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        Guid id)
    {
        var success = await _service.DeleteAsync(id);

        if (!success)
        {
            return NotFound(
                ApiResponse<string>.Fail("Customer not found"));
        }

        return Ok(ApiResponse<string>.Ok(
            "Deleted",
            "Deleted successfully"));
    }
}