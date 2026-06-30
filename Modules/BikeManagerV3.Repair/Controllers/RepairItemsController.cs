// Controllers/RepairItemsController.cs
using BikeManagerV3.Repair.DTOs.RepairItems;
using BikeManagerV3.Repair.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BikeManagerV3.Repair.Controllers;

[Authorize(AuthenticationSchemes = "OpenIddict.Validation.AspNetCore")]
[ApiController]
[IgnoreAntiforgeryToken]
[Route("api/v1/repair-items")]
public class RepairItemsController
    : ControllerBase
{
    private readonly IRepairItemService
        _service;

    public RepairItemsController(
        IRepairItemService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateRepairItemRequest request)
    {
        var result = await _service
            .CreateAsync(request);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery]
        RepairItemQuery query)
    {
        var result = await _service
            .GetAllAsync(query);

        return Ok(result);
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
        UpdateRepairItemRequest request)
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