using BikeManagerV3.Inventory.Services;
using BikeManagerV3.Inventory.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BikeManagerV3.Inventory.Responses;

namespace BikeManagerV3.Inventory.Controllers;


[Authorize(AuthenticationSchemes = "OpenIddict.Validation.AspNetCore")]
[ApiController]
[IgnoreAntiforgeryToken]
[Route("api/v1/inventory-stocks")]
public class InventoryStocksController
    : ControllerBase
{
    private readonly IInventoryStockService
        _inventoryStockService;

    public InventoryStocksController(
        IInventoryStockService inventoryStockService)
    {
        _inventoryStockService =
            inventoryStockService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateInventoryStockRequest request)
    {
        try
        {
            var result =
                await _inventoryStockService
                    .CreateAsync(request);

            return Ok(
                ApiResponse<InventoryStockResponse>.Ok(
                    result,
                    "Inventory stock created successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(
                ApiResponse<string>.Fail(ex.Message));
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] InventoryStockQuery query)
    {
        var result =
            await _inventoryStockService
                .GetAllAsync(query);

        return Ok(
            ApiResponse<List<InventoryStockResponse>>
                .Ok(result));
    }

    [HttpGet("detail")]
    public async Task<IActionResult> GetDetailAll(
        [FromQuery] InventoryStockQuery query)
    {
        var result =
            await _inventoryStockService
                .GetDetailAllAsync(query);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        Guid id)
    {
        var result =
            await _inventoryStockService
                .GetByIdAsync(id);

        if (result == null)
        {
            return NotFound(
                ApiResponse<string>.Fail(
                    "Inventory stock not found"));
        }

        return Ok(
            ApiResponse<InventoryStockResponse>
                .Ok(result));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateInventoryStockRequest request)
    {
        try
        {
            var result =
                await _inventoryStockService
                    .UpdateAsync(id, request);

            if (result == null)
            {
                return NotFound(
                    ApiResponse<string>.Fail(
                        "Inventory stock not found"));
            }

            return Ok(
                ApiResponse<InventoryStockResponse>
                    .Ok(
                        result,
                        "Updated successfully"));
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
            await _inventoryStockService
                .DeleteAsync(id);

        if (!result)
        {
            return NotFound(
                ApiResponse<string>.Fail(
                    "Inventory stock not found"));
        }

        return Ok(
            ApiResponse<string>.Ok(
                null,
                "Deleted successfully"));
    }
}