using BikeManagerV3.Inventory.DTOs.Warehouses;
using BikeManagerV3.Inventory.Responses;
using BikeManagerV3.Inventory.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BikeManagerV3.Inventory.Controllers;


[Authorize(AuthenticationSchemes = "OpenIddict.Validation.AspNetCore")]
[ApiController]
[IgnoreAntiforgeryToken]
[Route("api/v1/warehouses")]
public class WarehousesController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;

    public WarehousesController(
        IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateWarehouseRequest request)
    {
        try
        {
            var result =
                await _warehouseService.CreateAsync(request);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(
                ApiResponse<string>.Fail(ex.Message));
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] WarehouseQuery query)
    {
        var result =
            await _warehouseService.GetAllAsync(query);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        Guid id)
    {
        var result =
            await _warehouseService.GetByIdAsync(id);

        if (result == null)
        {
            return NotFound(
                ApiResponse<string>.Fail(
                    "Warehouse not found"));
        }

        return Ok(result);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateWarehouseRequest request)
    {
        try
        {
            var result =
                await _warehouseService.UpdateAsync(
                    id,
                    request);

            if (result == null)
            {
                return NotFound(
                    ApiResponse<string>.Fail(
                        "Warehouse not found"));
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(
                ApiResponse<string>.Fail(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        Guid id)
    {
        var result =
            await _warehouseService.DeleteAsync(id);

        if (!result)
        {
            return NotFound(
                ApiResponse<string>.Fail(
                    "Warehouse not found"));
        }

        return Ok(
            ApiResponse<string>.Ok(
                null,
                "Warehouse deleted successfully"));
    }
}