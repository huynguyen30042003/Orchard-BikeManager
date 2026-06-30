// Controllers/WarrantyClaimsController.cs
using Azure.Core;
using BikeManagerV3.Warranty.DTOs.WarrantyClaims;
using BikeManagerV3.Warranty.Responses;
using BikeManagerV3.Warranty.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BikeManagerV3.Warranty.Controllers;

[Authorize(AuthenticationSchemes = "OpenIddict.Validation.AspNetCore")]
[ApiController]
[IgnoreAntiforgeryToken]
[Route("api/v1/warranty-claims")]
public class WarrantyClaimsController : ControllerBase
{
    private readonly IWarrantyClaimService _service;

    public WarrantyClaimsController(
        IWarrantyClaimService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] WarrantyClaimQuery request)
    {
        var data = await _service.GetAllAsync(request);

        return Ok(ApiResponse<IEnumerable<WarrantyClaimDto>>
            .Ok(data));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var data = await _service.GetByIdAsync(id);

        if (data == null)
        {
            return NotFound(ApiResponse<string>
                .Fail("Warranty claim not found"));
        }

        return Ok(ApiResponse<WarrantyClaimDto>
            .Ok(data));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateWarrantyClaimDto dto)
    {
        try
        {
            var data = await _service.CreateAsync(dto);

            return Ok(ApiResponse<WarrantyClaimDto>
                .Ok(data, "Warranty claim created"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<string>
                .Fail(ex.Message));
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateWarrantyClaimDto dto)
    {
        try
        {
            var result = await _service.UpdateAsync(id, dto);

            if (!result)
            {
                return NotFound(ApiResponse<string>
                    .Fail("Warranty claim not found"));
            }

            return Ok(ApiResponse<string>
                .Ok("Updated"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<string>
                .Fail(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.DeleteAsync(id);

        if (!result)
        {
            return NotFound(ApiResponse<string>
                .Fail("Warranty claim not found"));
        }

        return Ok(ApiResponse<string>
            .Ok("Deleted"));
    }
}