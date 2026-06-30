// Controllers/WarrantiesController.cs
using BikeManagerV3.Warranty.DTOs.Warranties;
using BikeManagerV3.Warranty.Responses;
using BikeManagerV3.Warranty.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BikeManagerV3.Warranty.Controllers;

[Authorize(AuthenticationSchemes = "OpenIddict.Validation.AspNetCore")]
[ApiController]
[IgnoreAntiforgeryToken]
[Route("api/v1/warranties")]
public class WarrantiesController : ControllerBase
{
    private readonly IWarrantyService _service;

    public WarrantiesController(IWarrantyService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] WarrantyQuery query)
    {
        var result = await _service.GetAllAsync(query);

        return Ok(
          ApiResponse<PagedResult<WarrantyDto>>
          .Ok(result)
      );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var data = await _service.GetByIdAsync(id);

        if (data == null)
        {
            return NotFound(ApiResponse<string>
                .Fail("Warranty not found"));
        }

        return Ok(ApiResponse<WarrantyDto>
            .Ok(data));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateWarrantyDto dto)
    {
        try
        {
            var data = await _service.CreateAsync(dto);

            return Ok(ApiResponse<WarrantyDto>
                .Ok(data, "Warranty created"));
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
        UpdateWarrantyDto dto)
    {
        try
        {
            var result = await _service.UpdateAsync(id, dto);

            if (!result)
            {
                return NotFound(ApiResponse<string>
                    .Fail("Warranty not found"));
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
                .Fail("Warranty not found"));
        }

        return Ok(ApiResponse<string>
            .Ok("Deleted"));
    }
}