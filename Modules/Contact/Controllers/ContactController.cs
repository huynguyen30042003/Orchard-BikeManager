using Contact.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Contact.Controllers;

public class ContactController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IContactService _contactService;

    public ContactController(IConfiguration configuration, IContactService contactService)
    {
        _configuration = configuration;
        _contactService = contactService;
    }

    [HttpGet("contact")]
    public IActionResult Index()
    {
        ViewBag.SiteKey = _configuration["ReCaptcha:SiteKey"];
        return View("~/Areas/Contact/Views/Contact/Index.cshtml");
    }

    [HttpGet("manager/contact")]
    public IActionResult Manager()
    {
        var listContact = _contactService.GetAllAsync().Result;
        return View("~/Areas/Contact/Views/Manager/Index.cshtml", listContact);
    }
}