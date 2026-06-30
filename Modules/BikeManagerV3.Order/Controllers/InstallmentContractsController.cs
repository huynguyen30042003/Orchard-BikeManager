// Controllers/InstallmentContractsController.cs
using BikeManagerV3.Order.DTOs.InstallmentContracts;
using BikeManagerV3.Order.Responses;
using BikeManagerV3.Order.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikeManagerV3.Order.Controllers;


[Authorize(AuthenticationSchemes = "OpenIddict.Validation.AspNetCore")]
[ApiController]
[IgnoreAntiforgeryToken]
[Route("api/v1/installment-contracts")]
public class InstallmentContractsController
    : ControllerBase
{
    private readonly
        IInstallmentContractService _service;

    public InstallmentContractsController(
        IInstallmentContractService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateInstallmentContractRequest request)
    {
        return Ok(
            await _service.CreateAsync(request));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery]
        InstallmentContractQuery query)
    {
        var result = await _service.GetAllAsync(query);

        return Ok(result);

    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        Guid id)
    {
        var result = await _service
            .GetByIdAsync(id);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("orderId/{orderId}")]
    public async Task<IActionResult> GetByOrderId(
        Guid orderId)
    {
        var result = await _service
            .GetByOrderIdAsync(orderId);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateInstallmentContractRequest request)
    {
        var result = await _service
            .UpdateAsync(id, request);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        Guid id)
    {
        var result = await _service.DeleteAsync(id);

        if (!result.Success)
        {
            return NotFound(result);
        }

        return Ok(result);
    }
}