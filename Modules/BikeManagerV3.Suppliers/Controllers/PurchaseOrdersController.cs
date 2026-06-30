using BikeManagerV3.Suppliers.DTOs.PurchaseOrder;
using BikeManagerV3.Suppliers.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikeManagerV3.Suppliers.Controllers
{
    [IgnoreAntiforgeryToken]
    [Authorize(AuthenticationSchemes = "OpenIddict.Validation.AspNetCore")]
    [ApiController]
    [Route("api/v1/purchase-orders")]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly IPurchaseOrderService _service;

        public PurchaseOrdersController(
            IPurchaseOrderService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] PurchaseOrderQuery query)
        {
            return Ok(await _service.GetPagedAsync(query));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreatePurchaseOrderRequest request)
        {
            var userId = User.FindFirst("sub")!.Value;
            var result = await _service.CreateAsync(request, userId);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("{id:guid}/approve")]
        public async Task<IActionResult> Approve(Guid id)
        {
            var result =
                       await _service.ApproveAsync(id);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var result = await _service.CancelAsync(id);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("{id:guid}/receive")]
        public async Task<IActionResult> Receive(
            Guid id,
            [FromBody] ReceivePurchaseOrderRequest request)
        {
            var userId = User.FindFirst("sub")!.Value;
            Console.WriteLine($"UserId: {userId}");

            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"{claim.Type} = {claim.Value}");
            }
            var result = await _service.ReceiveAsync(
                id,
                request);

            return Ok(result);
        }

        [HttpPost("{id:guid}/apply")]
        public async Task<IActionResult> Apply(Guid id)
        {
            var result = await _service.ApplyAsync(id);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
