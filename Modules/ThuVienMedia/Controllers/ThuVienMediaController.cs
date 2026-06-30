using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThuVienMedia.DTOs.ThuVienMedia;
using ThuVienMedia.Services.Interfaces;

namespace ThuVienMedia.Controllers;

[ApiController]
[IgnoreAntiforgeryToken]
[Route("api/v1/thu-vien-media")]
public class ThuVienMediaController : ControllerBase
{
    private readonly IThuVienMediaService _service;

    public ThuVienMediaController(
        IThuVienMediaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] ThuVienMediaQuery query)
    {
        var result = await _service.GetAll(query);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id)
    {
        var result = await _service.GetById(id);

        return Ok(result);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create([FromForm] CreateThuVienMediaRequest request)
    {
        var result = await _service.CreateAsync(request);

        if (!result.Success)
        {
            return NotFound(result);
        }

        return StatusCode(201, result);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateThuVienMediaRequest request)
    {
        var result = await _service.UpdateAsync(id, request);

        if (!result.Success)
        {
            return NotFound(result);
        }

        return StatusCode(200, result);
    }

    [HttpPatch("{id:guid}/ThumbAnhDaiDien")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateThumbAnhDaiDien(Guid id, [FromForm] IFormFile ThumbAnhDaiDien)
    {
        var result = await _service.UpdateThumbAnhDaiDienAsync(id, ThumbAnhDaiDien);

        if (!result.Success)
        {
            return NotFound(result);
        }

        return StatusCode(200, result);
    }

    [HttpPatch("{id:guid}/AnhDaiDien")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateAnhDaiDien(Guid id, [FromForm] IFormFile AnhDaiDien)
    {
        var result = await _service.UpdateAnhDaiDienAsync(id, AnhDaiDien);

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
