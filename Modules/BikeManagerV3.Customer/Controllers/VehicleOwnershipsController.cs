using BikeManagerV3.Customer.DTOs.VehicleOwnerships;
using BikeManagerV3.Customer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[Authorize(AuthenticationSchemes = "OpenIddict.Validation.AspNetCore")]
[ApiController]
[IgnoreAntiforgeryToken]
[Route("api/v1/vehicle-ownerships")]
public class VehicleOwnershipsController
    : ControllerBase
{
    private readonly IVehicleOwnershipService _service;

    public VehicleOwnershipsController(
        IVehicleOwnershipService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateVehicleOwnershipRequest request)
    {
        return Ok(await _service.CreateAsync(request));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] VehicleOwnershipQuery query)
    {
        return Ok(await _service.GetAllAsync(query));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        Guid id)
    {
        var result = await _service.GetByIdAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateVehicleOwnershipRequest request)
    {
        var result = await _service.UpdateAsync(
            id,
            request);

        if (result == null)
            return NotFound();

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