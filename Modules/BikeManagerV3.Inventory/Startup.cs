using BikeManagerV3.Inventory.Data;
using BikeManagerV3.Inventory.Services;
//using BikeManagerV3.Product.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace BikeManagerV3.Inventory;

public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        var connectionString = "Server=.;Database=BikeManager_V3;Trusted_Connection=True;TrustServerCertificate=True";

        services.AddDbContext<InventoryDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IInventoryStockService, InventoryStockService>();
        services.AddScoped<IWarehouseService, WarehouseService>();
        services.AddScoped<IInventoryTransactionService, InventoryTransactionService>();
        services.AddScoped<IInventoryCostLayerService, InventoryCostLayerService>();
            //services.AddControllers();
    }
}