using Microsoft.AspNetCore.Mvc;
using Thoitiet.Interfaces;

namespace Thoitiet.Controllers
{
    public class WeatherViewController : Controller
    {
        private readonly IWeatherConfigService _service;

        public WeatherViewController(IWeatherConfigService service)
        {
            _service = service;
        }

        [HttpGet("quan-li-api-thoi-tiet")]
        public async Task<IActionResult> Index()
        {
            var weatherConfig = await _service.GetWeatherConfig();
            return View("~/Areas/Thoitiet/Views/Thoitiet/Index.cshtml", weatherConfig);
        }
    }
}
