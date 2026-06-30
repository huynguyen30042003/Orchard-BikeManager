using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using ThuVienMedia.Data;
using ThuVienMedia.Services;
using ThuVienMedia.Services.Interfaces;

namespace ThuVienMedia;

public sealed class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ThuVienDbContext>(options =>
        {
            options.UseSqlServer("Server=.;Database=BikeManager_V3;Trusted_Connection=True;TrustServerCertificate=True");
        });

        services.AddScoped<ICMThuVienMediaService, CMThuVienMediaService>();
        services.AddScoped<IThuVienMediaService, ThuVienMediaService>();
        services.AddScoped<IFileMediaService, FileMediaService>();
    }
}

