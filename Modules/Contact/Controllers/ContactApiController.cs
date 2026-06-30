using Contact.Models;
using Contact.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Contact.Controllers;

[Route("api/contact")]
[ApiController]
[IgnoreAntiforgeryToken]
public class ContactApiController : ControllerBase
{
    private readonly IContactService _service;
    public ContactApiController(IContactService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(ContactRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName))
        {
            return BadRequest(new { message = "Vui lòng nhập họ tên" });
        }

        if (string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            return BadRequest(new { message = "Vui lòng nhập số điện thoại" });
        }
        var phone = request.PhoneNumber.Trim();

        if (!System.Text.RegularExpressions.Regex.IsMatch(
                phone,
                @"^(0|\+84)(3|5|7|8|9)\d{8}$"))
        {
            return BadRequest(new { message = "Số điện thoại không hợp lệ" });
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest(new { message = "Vui lòng nhập email" });
        }

        if (!string.IsNullOrWhiteSpace(request.Email)
            && !new EmailAddressAttribute().IsValid(request.Email))
        {
            return BadRequest(new { message = "Email không hợp lệ" });
        }

        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return BadRequest(new { message = "Vui lòng nhập tiêu đề" });
        }

        if (string.IsNullOrWhiteSpace(request.Content))
        {
            return BadRequest(new { message = "Vui lòng nhập nội dung" });
        }

        await _service.CreateAsync(request);

        return Ok(new { message = "Gửi thành công" });
    }

    [HttpPatch("reply")]
    public async Task<IActionResult> Update(ContactReplyRequest request)
    {
        await _service.ReplyAsync(request);
        return Ok(new
        {
            message = "Trả lời thành công"
        });
    }

    [HttpPost("sendEmail/{id}")]
    public async Task<IActionResult> Send(Guid id)
    {
        await _service.SendAsync(id);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var status = await _service.DeleteAsync(id);

        if (!status)
        {
            return NotFound(new { message = "Liên hệ không tồn tại" });
        }
        return Ok(new
        {
            message = "Xóa thành công"
        });
    }
}
