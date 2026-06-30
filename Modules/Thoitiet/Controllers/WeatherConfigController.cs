using Microsoft.AspNetCore.Mvc;
using Thoitiet.DTOs;
using Thoitiet.Interfaces;
using Thoitiet.Models;

[ApiController]
[Route("api/v1/weather")]
[IgnoreAntiforgeryToken]
public class WeatherConfigController
    : ControllerBase
{
    private readonly
    IWeatherConfigService
    _configService;

    public WeatherConfigController(

        IWeatherConfigService
        configService)

    {
        _configService =
            configService;
    }

    [HttpPost("config")]
    public async Task<IActionResult>
    SaveConfig(
    [FromBody]
    WeatherConfig config
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(
                ModelState
            );
        }
        await _configService
            .Save(config);

        return Ok();
    }
}