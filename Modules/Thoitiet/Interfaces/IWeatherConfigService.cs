using Thoitiet.DTOs;
using Thoitiet.Models;

namespace Thoitiet.Interfaces;

public interface IWeatherConfigService
{
    Task Save(
        WeatherConfig dto
    );

    Task<List<WeatherResponse>>
        GetWeather();
    Task<WeatherConfig>
        GetWeatherConfig();
}