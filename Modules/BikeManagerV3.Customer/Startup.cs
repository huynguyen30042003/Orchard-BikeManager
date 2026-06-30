using BikeManagerV3.Customer.Services;
using BikeManagerV3.Customer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace BikeManagerV3.Customer;

public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<CustomerDbContext>(options =>
        {
            options.UseSqlServer("Server=.;Database=BikeManager_V3;Trusted_Connection=True;TrustServerCertificate=True");
        });

        services.AddScoped<ICustomerService,CustomerService>();
        services.AddScoped<ICustomerStatisticService, CustomerStatisticService>();
        services.AddScoped<ICustomerVehicleService, CustomerVehicleService>();
        services.AddScoped<IVehicleOwnershipService, VehicleOwnershipService>();
        services.AddControllers();
    }
}