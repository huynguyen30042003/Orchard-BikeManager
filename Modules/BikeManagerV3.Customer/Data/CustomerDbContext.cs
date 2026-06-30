using BikeManagerV3.Customer.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Customer.Data;

public class CustomerDbContext : DbContext
{
    public CustomerDbContext(
        DbContextOptions<CustomerDbContext> options)
        : base(options)
    {
    }

    public DbSet<CustomerModel> Customers => Set<CustomerModel>();

    public DbSet<CustomerStatistic> CustomerStatistics
        => Set<CustomerStatistic>();

    public DbSet<CustomerVehicle> CustomerVehicles
        => Set<CustomerVehicle>();

    public DbSet<VehicleOwnership> VehicleOwnerships
        => Set<VehicleOwnership>();

    protected override void OnModelCreating(
        ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<CustomerModel>(entity =>
        {
            entity.ToTable("customers");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.FullName)
                .HasMaxLength(255);

            entity.Property(x => x.PhoneNumber)
                .HasMaxLength(20);

            entity.Property(x => x.Email)
                .HasMaxLength(255);

            entity.Property(x => x.Gender)
                .HasMaxLength(20);

            entity.Property(x => x.TotalSpent)
                .HasColumnType("decimal(18,2)");

            // 1 - 1
            entity.HasOne(x => x.Statistic)
                .WithOne(x => x.Customer)
                .HasForeignKey<CustomerStatistic>(
                    x => x.CustomerId);

            // 1 - many
            entity.HasMany(x => x.Vehicles)
                .WithOne(x => x.Customer)
                .HasForeignKey(x => x.CustomerId);

            entity.HasMany(x => x.VehicleOwnerships)
                .WithOne(x => x.Customer)
                .HasForeignKey(x => x.CustomerId);
        });

        // customer_statistics
        builder.Entity<CustomerStatistic>(entity =>
        {
            entity.ToTable("customer_statistics");

            entity.HasKey(x => x.CustomerId);

            entity.Property(x => x.TotalSpent)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.CustomerLevel)
                .HasMaxLength(50);

            entity.Property(x => x.DiscountRate)
                .HasColumnType("decimal(5,2)");
        });

        // customer_vehicles
        builder.Entity<CustomerVehicle>(entity =>
        {
            entity.ToTable("customer_vehicles");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.ModelName)
                .HasMaxLength(255);

            entity.Property(x => x.PlateNumber)
                .HasMaxLength(50);

            entity.Property(x => x.FrameNumber)
                .HasMaxLength(255);

            entity.Property(x => x.EngineNumber)
                .HasMaxLength(255);

            entity.Property(x => x.BatterySerial)
                .HasMaxLength(255);
        });

        // vehicle_ownerships
        builder.Entity<VehicleOwnership>(entity =>
        {
            entity.ToTable("vehicle_ownerships");

            entity.HasKey(x => x.Id);
        });
    }
}