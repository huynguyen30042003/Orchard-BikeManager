// Data/RepairDbContext.cs
using BikeManagerV3.Repair.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Repair.Data;

public class RepairDbContext : DbContext
{
    public RepairDbContext(
        DbContextOptions<RepairDbContext> options)
        : base(options)
    {
    }

    public DbSet<Service> Services
        => Set<Service>();

    public DbSet<RepairOrder> RepairOrders
        => Set<RepairOrder>();

    public DbSet<RepairItem> RepairItems
        => Set<RepairItem>();

    protected override void OnModelCreating(
        ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Service>(entity =>
        {
            entity.ToTable("services");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Code)
                .HasMaxLength(100);

            entity.Property(x => x.Name)
                .HasMaxLength(255);

            entity.Property(x => x.BasePrice)
                .HasColumnType("decimal(18,2)");
        });

        builder.Entity<RepairOrder>(entity =>
        {
            entity.ToTable("repair_orders");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.RepairCode)
                .HasMaxLength(100);

            entity.Property(x => x.Status)
                .HasMaxLength(50);

            entity.Property(x => x.EstimatedCost)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.TotalCost)
                .HasColumnType("decimal(18,2)");
        });

        builder.Entity<RepairItem>(entity =>
        {
            entity.ToTable("repair_items");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.ItemType)
                .HasMaxLength(50);

            entity.Property(x => x.UnitPrice)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.TotalPrice)
                .HasColumnType("decimal(18,2)");
        });
    }
}