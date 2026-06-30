// Controllers/InstallmentProvidersController.cs
using BikeManagerV3.Order.DTOs.InstallmentProviders;
using BikeManagerV3.Order.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BikeManagerV3.Order.Controllers;


[Authorize(AuthenticationSchemes = "OpenIddict.Validation.AspNetCore")]
[ApiController]
[IgnoreAntiforgeryToken]
[Route("api/v1/installment-providers")]
public class InstallmentProvidersController
    : ControllerBase
{
    private readonly
        IInstallmentProviderService _service;

    public InstallmentProvidersController(
        IInstallmentProviderService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateInstallmentProviderRequest request)
    {
        return Ok(
            await _service.CreateAsync(request));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery]
        InstallmentProviderQuery query)
    {
        return Ok(
            await _service.GetAllAsync(query));
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

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateInstallmentProviderRequest request)
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