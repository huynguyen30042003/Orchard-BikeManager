using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using Thoitiet.Caching;
using Thoitiet.Data;
using Thoitiet.Interfaces;
using Thoitiet.Services;

namespace Thoitiet;

public sealed class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {

        services.AddDbContext<ThoiTietDbContext>(options =>
        {
            options.UseSqlServer("Server=.;Database=BikeManager_V3;Trusted_Connection=True;TrustServerCertificate=True");
        });
        services.AddScoped<IWeatherConfigService, WeatherConfigService>();
        services.AddScoped<ICacheService, RedisCacheService>();
    }
}

