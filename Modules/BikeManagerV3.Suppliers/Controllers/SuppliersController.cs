using BikeManagerV3.Suppliers.DTOs.Supplier;
using BikeManagerV3.Suppliers.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikeManagerV3.Suppliers.Controllers
{
    [IgnoreAntiforgeryToken]
    [Authorize(AuthenticationSchemes = "OpenIddict.Validation.AspNetCore")]
    [ApiController]
    [Route("api/v1/suppliers")]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(
            ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] SupplierQuery query)
        {
            var result =
                await _supplierService.GetPagedAsync(query);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            Guid id)
        {
            var result =
                await _supplierService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateSupplierRequest request)
        {
            var result =
                await _supplierService.CreateAsync(request);

            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateSupplierRequest request)
        {
            var result =
                await _supplierService.UpdateAsync(id, request);

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(
            Guid id)
        {
            await _supplierService.DeleteAsync(id);

            return NoContent();
        }
    }
}