using BikeManagerV3.Order.Data;
using BikeManagerV3.Order.Service.Interfaces;
using BikeManagerV3.Order.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace BikeManagerV3.Order;

public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<OrderDbContext>(options =>
        {
            options.UseSqlServer("Server=.;Database=BikeManager_V3;Trusted_Connection=True;TrustServerCertificate=True");
        });

        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderItemService, OrderItemService>();
        services.AddScoped<IInstallmentProviderService, InstallmentProviderService>();
        services.AddScoped<IInstallmentContractService, InstallmentContractService>();
        services.AddScoped<ISalesWorkflowService, SalesWorkflowService>();
        //services.AddControllers();
    }
}