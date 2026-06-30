using BikeManagerV3.Order.DTOs.Sales;
using BikeManagerV3.Order.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikeManagerV3.Order.Controllers;

[Authorize(AuthenticationSchemes = "OpenIddict.Validation.AspNetCore")]
[ApiController]
[IgnoreAntiforgeryToken]
[Route("api/v1/sales")]
public class SalesWorkflowController : ControllerBase
{
    private readonly ISalesWorkflowService _service;

    public SalesWorkflowController(
        ISalesWorkflowService service)
    {
        _service = service;
    }

    [HttpPost("create-order")]
    public async Task<IActionResult> CreateOrder(
        [FromBody] CreateSaleOrderRequest request)
    {
        var result = await _service.CreateSaleOrderAsync(request);
        return Ok(result);
    }

    [HttpPost("create-installment-order")]
    public async Task<IActionResult> CreateInstallmentOrder(
        [FromBody] CreateInstallmentOrderRequest request)
    {
        var result = await _service.CreateInstallmentOrderAsync(request);
        return Ok(result);
    }

    [HttpPost("{orderId:guid}/cancel")]
    public async Task<IActionResult> CancelOrder(
        [FromRoute] Guid orderId)
    {
        await _service.CancelOrderAsync(orderId);
        return Ok();
    }

    [HttpPost("{orderId:guid}/return")]
    public async Task<IActionResult> ReturnOrder(
        [FromRoute] Guid orderId)
    {
        await _service.ReturnOrderAsync(orderId);
        return Ok();
    }
}