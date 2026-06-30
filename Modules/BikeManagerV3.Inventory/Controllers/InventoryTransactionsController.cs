using BikeManagerV3.Inventory.DTOs.InventoryTransactions;
using BikeManagerV3.Inventory.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BikeManagerV3.Inventory.Responses;

namespace BikeManagerV3.Inventory.Controllers;


[Authorize(AuthenticationSchemes = "OpenIddict.Validation.AspNetCore")]
[ApiController]
[IgnoreAntiforgeryToken]
[Route("api/v1/inventory-transactions")]
public class InventoryTransactionsController : ControllerBase
{
    private readonly IInventoryTransactionService _service;

    public InventoryTransactionsController(
        IInventoryTransactionService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateInventoryTransactionRequest request)
    {
        var result = await _service.CreateAsync(request);

        return Ok(ApiResponse<InventoryTransactionResponse>.Ok(
            result,
            "Created successfully"));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] InventoryTransactionQuery query)
    {
        var result = await _service.GetAllAsync(query);

        return Ok(ApiResponse<List<InventoryTransactionResponse>>.Ok(
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

        return Ok(ApiResponse<InventoryTransactionResponse>.Ok(
            result));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateInventoryTransactionRequest request)
    {
        var result = await _service.UpdateAsync(id, request);

        if (result == null)
        {
            return NotFound(
                ApiResponse<string>.Fail("Not found"));
        }

        return Ok(ApiResponse<InventoryTransactionResponse>.Ok(
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