using Microsoft.AspNetCore.Mvc;
using ThuVienMedia.DTOs.FileMedia;
using ThuVienMedia.Responses;
using ThuVienMedia.Services.Interfaces;

namespace ThuVienMedia.Controllers;

[ApiController]
[IgnoreAntiforgeryToken]
[Route("api/v1/file")]
public class FileMediaController : ControllerBase
{
    private readonly IFileMediaService _service;
    public FileMediaController(
      IFileMediaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged(
      [FromQuery] FileMediaQuery query)
    {
        var result = await _service.GetPagedAsync(query);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);

        if (result == null)
            return NotFound(ApiResponse<FileMediaResponse>.Fail("Không tìm thấy dữ liệu."));

        return Ok(result);
    }

    [HttpPost("{ThuVienId:guid}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create([FromForm] Guid ThuVienId, CreateFileMediaRequest request)
    {
        var result = await _service.CreateAsync(ThuVienId, request);

        if (!result.Success)
        {
            return NotFound(result);
        }

        return StatusCode(201, result);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateInfoFileMediaRequest request)
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
