// Controllers/RepairOrdersController.cs
using BikeManagerV3.Repair.DTOs.RepairOrders;
using BikeManagerV3.Repair.Responses;
using BikeManagerV3.Repair.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikeManagerV3.Repair.Controllers;

[Authorize(AuthenticationSchemes = "OpenIddict.Validation.AspNetCore")]
[ApiController]
[IgnoreAntiforgeryToken]
[Route("api/v1/repair-orders")]
public class RepairOrdersController
    : ControllerBase
{
    private readonly IRepairOrderService
        _service;

    public RepairOrdersController(
        IRepairOrderService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateRepairOrderRequest request)
    {
        var result = await _service
            .CreateAsync(request);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery]
        RepairOrderQuery query)
    {
        var result = await _service.GetAllAsync(query);

        return Ok(
          ApiResponse<PagedResult<RepairOrderResponse>>
          .Ok(result)
      );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        Guid id)
    {
        var result = await _service
            .GetByIdAsync(id);

        if (!result.Success)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateRepairOrderRequest request)
    {
        var result = await _service
            .UpdateAsync(id, request);

        if (!result.Success)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        Guid id)
    {
        var result = await _service
            .DeleteAsync(id);

        if (!result.Success)
        {
            return NotFound(result);
        }

        return Ok(result);
    }
}