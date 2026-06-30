using BikeManagerV3.Product.DTOs.SerialNumber;
using BikeManagerV3.Product.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BikeManagerV3.Product.Controllers;

[Authorize(AuthenticationSchemes = "OpenIddict.Validation.AspNetCore")]
[IgnoreAntiforgeryToken]
[ApiController]
[Route("api/v1/serial-numbers")]
public class SerialNumberController : ControllerBase
{
    private readonly ISerialNumberService _service;

    public SerialNumberController(
        ISerialNumberService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] SerialNumberQuery query)
    {
        var result =
            await _service.GetAll(query);


        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        Guid id)
    {
        var result =
            await _service.GetById(id);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody]
        CreateSerialNumberRequest request)
    {
        var result =
            await _service.CreateSerial(request);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody]
        UpdateSerialNumberRequest request)
    {
        var result =
            await _service.Update(id, request);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        Guid id)
    {
        var result =
            await _service.Delete(id);

        return Ok(result);
    }
}