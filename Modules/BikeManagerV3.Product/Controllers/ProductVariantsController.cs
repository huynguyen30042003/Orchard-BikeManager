using BikeManagerV3.Product.DTOs.ProductVariant;
using BikeManagerV3.Product.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BikeManagerV3.Product.Controllers;

[Authorize(AuthenticationSchemes = "OpenIddict.Validation.AspNetCore")]
[IgnoreAntiforgeryToken]
[ApiController]
[Route("api/v1/product-variants")]
public class ProductVariantController
    : ControllerBase
{
    private readonly IProductVariantService _service;

    public ProductVariantController(
        IProductVariantService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] ProductVariantQuery query)
    {
        var result =
            await _service.GetAll(query);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id)
    {
        var result =
            await _service.GetById(id);

        return Ok(result);
    }

    [HttpGet("product/{id:guid}")]
    public async Task<IActionResult> GetByProductId(
    Guid productId)
    {
        var result =
            await _service.GetByProductId(productId);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody]
        CreateProductVariantRequest request)
    {
        var result =
            await _service.Create(request);

        return Ok(result);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody]
        UpdateProductVariantRequest request)
    {
        var result =
            await _service.Update(id, request);

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id)
    {
        var result =
            await _service.Delete(id);

        return Ok(result);
    }


    [HttpGet("productDashboard")]
    public async Task<IActionResult> GetDashboardProduct()
    {
        var result =
            await _service.GetDashboardProduct();
        return Ok(result);
    }
}