namespace Thoitiet.Services
{
    using Microsoft.EntityFrameworkCore;
    using Thoitiet.Caching;
    using Thoitiet.Data;
    using Thoitiet.DTOs;
    using Thoitiet.Interfaces;
    using Thoitiet.Models;

    public class WeatherConfigService
        : IWeatherConfigService
    {
        private readonly ICacheService
            _cache;
        private readonly ThoiTietDbContext
            _dbContext;
        private readonly OpenMeteoClient
        _openMeteoClient;

        public WeatherConfigService(
            ThoiTietDbContext dbContext,
            ICacheService cache,
            OpenMeteoClient openMeteoClient)
        {
            _cache = cache;
            _dbContext = dbContext;
            _openMeteoClient = openMeteoClient;
        }

        public async Task Save(
            WeatherConfig dto)
        {
            var entity =
                new WeatherConfiguration
                {
                    PositionName =
                        string.Join(
                        ",",
                        dto.PositionName),
                    Latitude =
                        string.Join(
                        ",",
                        dto.Latitude),

                    Longitude =
                        string.Join(
                        ",",
                        dto.Longitude),

                    Daily =
                        string.Join(
                        ",",
                        dto.Daily),

                    Hourly =
                        string.Join(
                        ",",
                        dto.Hourly),

                    Current =
                        string.Join(
                        ",",
                        dto.Current),

                    Timezone =
                        dto.Timezone,

                    CreatedAt =
                        DateTime.UtcNow,

                    UpdatedAt =
                        DateTime.UtcNow
                };
            _dbContext
                .WeatherConfigurations
                .Add(entity);
            await _dbContext
                .SaveChangesAsync();
        }

        public async Task<List<WeatherResponse>>
        GetWeather()
        {
            var config =
                await _dbContext
                .WeatherConfigurations
                .OrderByDescending(x => x.UpdatedAt)
                .FirstAsync();
            var cacheKey = $"weather:{config.Id}";
            return await _cache
                .GetOrCreateAsync(
                cacheKey,
                async () =>
                {
                    return await
                    _openMeteoClient
                    .GetWeather(config);
                },
                TimeSpan
                .FromMinutes(15)
            );
        }

        public async Task<WeatherConfig>
        GetWeatherConfig()
        {
            var config =
             await _dbContext
            .WeatherConfigurations
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();

            if (config == null)
            {
                return null;
            }
            Console.WriteLine("configconfig");
            Console.WriteLine(config.PositionName);
            Console.WriteLine("LatitudeLatitude");
            Console.WriteLine(config.Latitude);
            Console.WriteLine("LongitudeLongitude");
            Console.WriteLine(config.Longitude);

            return new WeatherConfig
            {
                Id = config.Id,
                PositionName = config.PositionName != null ?
                    config.PositionName
                    .Split(',')
                    .ToList() : [],

                Latitude =
                    config.Latitude
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .ToList(),

                Longitude =
                    config.Longitude
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .ToList(),

                Daily =
                    config.Daily
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .ToList(),

                Hourly =
                    config.Hourly
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .ToList(),

                Current =
                    config.Current
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .ToList(),

                Timezone = config.Timezone,

                CreatedAt = config.CreatedAt,

                UpdatedAt = config.UpdatedAt
            };
        }
    }
}
