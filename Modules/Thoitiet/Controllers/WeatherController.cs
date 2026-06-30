using Microsoft.AspNetCore.Mvc;
using Thoitiet.Interfaces;

[ApiController]

[Route("api/v1/weather")]

public class WeatherController
    : ControllerBase
{
    private readonly IWeatherConfigService _service;

    public WeatherController(IWeatherConfigService service)
    {
        _service = service;
    }

    [HttpGet]

    public async Task<IActionResult>

    Get()
    {
        return Ok(
        await _service
        .GetWeather()
        );
    }

    [HttpGet("test")]
    public async Task<IActionResult>
    test()
    {
        var result = await _service
        .GetWeatherConfig();
        return Ok(result);
    }
}