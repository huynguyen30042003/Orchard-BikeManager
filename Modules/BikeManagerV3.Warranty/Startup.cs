using BikeManagerV3.Warranty.Data;
using BikeManagerV3.Warranty.Services;
using BikeManagerV3.Warranty.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace BikeManager.Repair;

public sealed class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<WarrantyDbContext>(options =>
        {
            options.UseSqlServer("Server=.;Database=BikeManager_V3;Trusted_Connection=True;TrustServerCertificate=True");
        });

        services.AddScoped<IWarrantyClaimService, WarrantyClaimService>();
        services.AddScoped<IWarrantyService, WarrantyService>();
            //services.AddControllers();
    }
}

