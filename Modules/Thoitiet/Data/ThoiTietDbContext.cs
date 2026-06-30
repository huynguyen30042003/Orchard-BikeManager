using Microsoft.EntityFrameworkCore;
using Thoitiet.Models;

namespace Thoitiet.Data;

public class ThoiTietDbContext : DbContext
{
    public ThoiTietDbContext(
        DbContextOptions<ThoiTietDbContext> options)
        : base(options)
    {
    }

    public DbSet<WeatherConfiguration> WeatherConfigurations
    => Set<WeatherConfiguration>();

    protected override void OnModelCreating(
        ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}