using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using Microsoft.EntityFrameworkCore;
using BikeManagerV3.Auth.Data;
namespace BikeManagerV3.Auth
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UserDbContext>(options =>
            options.UseSqlServer(
                "Server=localhost;Database=BikeManager_V3;Trusted_Connection=True;TrustServerCertificate=True"
            ));
            services.AddControllers();
        }
    }
}