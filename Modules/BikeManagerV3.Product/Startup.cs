using BikeManagerV3.Counters.Services;
using BikeManagerV3.Product.Controllers;
using BikeManagerV3.Product.Data;
using BikeManagerV3.Product.Services;
using BikeManagerV3.Product.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace BikeManagerV3.Product;

public sealed class Startup : StartupBase
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<CatalogDbContext>(options =>
            options.UseSqlServer(
                _configuration.GetConnectionString("Default")));

        services.AddScoped<IProductImageService, ProductImageService>();
        services.AddScoped<IProductVariantService, ProductVariantService>();
        services.AddScoped<ISerialNumberService, SerialNumberService>();
        services.AddScoped<ICounterService, CounterService>();
    }
}