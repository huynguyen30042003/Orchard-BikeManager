using BikeManagerV3.Customer.DTOs.CustomerStatistics;
using BikeManagerV3.Customer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(AuthenticationSchemes = "OpenIddict.Validation.AspNetCore")]
[ApiController]
[IgnoreAntiforgeryToken]
[Route("api/v1/customer-statistics")]
public class CustomerStatisticsController
    : ControllerBase
{
    private readonly ICustomerStatisticService _service;

    public CustomerStatisticsController(
        ICustomerStatisticService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateCustomerStatisticRequest request)
    {
        var result = await _service.CreateAsync(request);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] CustomerStatisticQuery query)
    {
        return Ok(await _service.GetAllAsync(query));
    }

    [HttpGet("{customerId}")]
    public async Task<IActionResult> GetById(
        Guid customerId)
    {
        var result = await _service
            .GetByIdAsync(customerId);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPut("{customerId}")]
    public async Task<IActionResult> Update(
        Guid customerId,
        UpdateCustomerStatisticRequest request)
    {
        var result = await _service.UpdateAsync(
            customerId,
            request);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{customerId}")]
    public async Task<IActionResult> Delete(
        Guid customerId)
    {
        var success = await _service
            .DeleteAsync(customerId);

        if (!success)
            return NotFound();

        return Ok();
    }
}