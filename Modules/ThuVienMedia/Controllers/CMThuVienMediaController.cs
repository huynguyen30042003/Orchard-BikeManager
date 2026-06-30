using Microsoft.AspNetCore.Mvc;
using ThuVienMedia.DTOs.CMThuVienMedia;
using ThuVienMedia.Services.Interfaces;

namespace ThuVienMedia.Controllers;

[ApiController]
[IgnoreAntiforgeryToken]
[Route("api/v1/CM-thu-vien-media")]
public class CMThuVienMediaController : ControllerBase
{
    private readonly ICMThuVienMediaService _service;

    public CMThuVienMediaController(
        ICMThuVienMediaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] CMThuVienMediaQuery query)
    {
        var result = await _service.GetAll(query);

        return Ok(result);
    }

    [HttpGet("tree")]
    public async Task<IActionResult> GetTree(
        [FromQuery] CMThuVienMediaQuery query)
    {
        var result = await _service.GetAllTree(query);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);

        if (!result.Success)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCMThuVienMediaRequest request)
    {
        var result = await _service.CreateAsync(request);

        if (!result.Success)
        {
            return NotFound(result);
        }

        return StatusCode(201, result);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateCMThuVienMediaRequest request)
    {
        var result = await _service.UpdateAsync(id, request);

        if (!result.Success)
        {
            return NotFound(result);
        }

        return StatusCode(200, result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.DeleteAsync(id);

        if (!result.Success)
        {
            return NotFound(result);
        }

        return StatusCode(204, result);
    }
}