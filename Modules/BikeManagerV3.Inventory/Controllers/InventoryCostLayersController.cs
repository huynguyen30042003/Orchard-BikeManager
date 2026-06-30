using BikeManagerV3.Inventory.DTOs.InventoryCostLayers;
using BikeManagerV3.Inventory.Responses;
using BikeManagerV3.Inventory.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikeManagerV3.Inventory.Controllers;


[Authorize(AuthenticationSchemes = "OpenIddict.Validation.AspNetCore")]
[ApiController]
[IgnoreAntiforgeryToken]
[Route("api/v1/inventory-cost-layers")]
public class InventoryCostLayersController
    : ControllerBase
{
    private readonly IInventoryCostLayerService _service;

    public InventoryCostLayersController(
        IInventoryCostLayerService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateInventoryCostLayerRequest request)
    {
        var result = await _service.CreateAsync(request);

        return Ok(ApiResponse<InventoryCostLayerResponse>.Ok(
            result,
            "Created successfully"));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] InventoryCostLayerQuery query)
    {
        var result = await _service.GetAllAsync(query);

        return Ok(ApiResponse<List<InventoryCostLayerResponse>>.Ok(
            result));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        Guid id)
    {
        var result = await _service.GetByIdAsync(id);

        if (result == null)
        {
            return NotFound(
                ApiResponse<string>.Fail("Not found"));
        }

        return Ok(ApiResponse<InventoryCostLayerResponse>.Ok(
            result));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateInventoryCostLayerRequest request)
    {
        var result = await _service.UpdateAsync(id, request);

        if (result == null)
        {
            return NotFound(
                ApiResponse<string>.Fail("Not found"));
        }

        return Ok(ApiResponse<InventoryCostLayerResponse>.Ok(
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
                ApiResponse<string>.Fail("Not found"));
        }

        return Ok(ApiResponse<string>.Ok(
            "Deleted",
            "Deleted successfully"));
    }
}