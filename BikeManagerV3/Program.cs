using BikeManagerV3.Contact.Data;
using BikeManagerV3.Customer.Data;
using BikeManagerV3.Inventory.Data;
using BikeManagerV3.Order.Data;
using BikeManagerV3.Product.Data;
using BikeManagerV3.Repair.Data;
using BikeManagerV3.Suppliers.Data;
using BikeManagerV3.Warranty.Data;
using Microsoft.EntityFrameworkCore;
using OrchardCore.Logging;
using Thoitiet.Data;
using ThuVienMedia.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
builder.Services.AddScoped<OpenMeteoClient>();
builder.Host.UseNLogHost();
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowFrontend",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration =
        builder.Configuration[
            "Redis:ConnectionString"
        ];
});

builder.Services.AddOrchardCms();

builder.Services.AddOpenIddict()
    .AddServer(options =>
    {
        options.SetAccessTokenLifetime(TimeSpan.FromHours(1));

        options.SetRefreshTokenLifetime(TimeSpan.FromDays(30));
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();
builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default")));

builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default"))
        .EnableSensitiveDataLogging()
        .LogTo(Console.WriteLine));

builder.Services.AddDbContext<CustomerDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default")));

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default")));

builder.Services.AddDbContext<RepairDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default")));

builder.Services.AddDbContext<WarrantyDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default")));

builder.Services.AddDbContext<SuppliersDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default")));

builder.Services.AddDbContext<ContactDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default")));

builder.Services.AddDbContext<
    ThoiTietDbContext>(options =>
    {
        options.UseSqlServer(
            builder.Configuration
            .GetConnectionString(
                "Default"));
    });
//builder.Services.AddDbContext<
//   ThuVienDbContext>(options =>
//    {
//        options.UseSqlServer(
//            builder.Configuration
//            .GetConnectionString(
//                "Default"));
//    });


var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowFrontend");

app.UseAuthentication();

app.UseAuthorization();

app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;

    if (response.HasStarted)
        return;

    response.ContentType = "application/json";

    switch (response.StatusCode)
    {
        case 401:

            await response.WriteAsJsonAsync(new
            {
                success = false,
                message = "Unauthorized"
            });

            break;

        case 403:

            await response.WriteAsJsonAsync(new
            {
                success = false,
                message = "Forbidden"
            });

            break;
    }
});
app.UseStaticFiles();

app.UseOrchardCore();

app.Run();