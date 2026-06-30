using BikeManagerV3.Product.DTOs.ProductImage;
using BikeManagerV3.Product.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BikeManagerV3.Product.Controllers;

[Authorize(AuthenticationSchemes = "OpenIddict.Validation.AspNetCore")]
[IgnoreAntiforgeryToken]
[ApiController]
[Route("api/v1/product-images")]
public class ProductImagesController : ControllerBase
{
    private readonly IProductImageService _service;

    public ProductImagesController(
        IProductImageService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid? productId,
        [FromQuery] bool? isThumbnail)
    {
        var result = await _service.GetAll(
            productId,
            isThumbnail);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetById(id);

        return Ok(result);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create(
        [FromForm] CreateProductImageRequest request)
    {
        var result = await _service.Create(request);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromForm] UpdateProductImageRequest request)
    {
        var result = await _service.Update(id, request);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        Guid id)
    {
        var result = await _service.Delete(id);

        return Ok(result);
    }
}