using BikeManagerV3.Customer.DTOs.Customers;
using BikeManagerV3.Customer.DTOs.CustomerVehicles;
using BikeManagerV3.Customer.Responses;
using BikeManagerV3.Customer.Services;
using Castle.Core.Resource;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(AuthenticationSchemes = "OpenIddict.Validation.AspNetCore")]
[ApiController]
[IgnoreAntiforgeryToken]
[Route("api/v1/customer-vehicles")]
public class CustomerVehiclesController
    : ControllerBase
{
    private readonly ICustomerVehicleService _service;

    public CustomerVehiclesController(
        ICustomerVehicleService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateCustomerVehicleRequest request)
    {
        return Ok(await _service.CreateAsync(request));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] CustomerVehicleQuery query)
    {
        var result = await _service.GetAllAsync(query);

        return Ok(
          ApiResponse<PagedResult<CustomerVehicleResponse>>
          .Ok(result)
      );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        Guid id)
    {
        var result = await _service.GetByIdAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateCustomerVehicleRequest request)
    {
        var result = await _service.UpdateAsync(
            id,
            request);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        Guid id)
    {
        var success = await _service.DeleteAsync(id);

        if (!success.Success)
        {
            return NotFound(
                ApiResponse<string>.Fail("Not found"));
        }

        return Ok(ApiResponse<string>.Ok(
            "Deleted",
            "Deleted successfully"));
    }
}