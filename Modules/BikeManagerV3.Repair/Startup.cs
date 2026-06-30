using BikeManagerV3.Repair.Data;
using BikeManagerV3.Repair.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace BikeManagerV3.Repair;

public sealed class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<RepairDbContext>(options =>
        {
            options.UseSqlServer("Server=.;Database=BikeManager_V3;Trusted_Connection=True;TrustServerCertificate=True");
        });

        services.AddScoped<IRepairItemService, RepairItemService>();
        services.AddScoped<IRepairOrderService, RepairOrderService>();
        services.AddScoped<IServiceService, ServiceService>();
            //services.AddControllers();
    }
}

