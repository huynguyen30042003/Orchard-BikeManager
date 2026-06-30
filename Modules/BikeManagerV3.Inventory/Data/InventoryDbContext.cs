using BikeManagerV3.Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Inventory.Data;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(
        DbContextOptions<InventoryDbContext> options)
        : base(options)
    {
    }
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();

    public DbSet<InventoryStock> InventoryStocks
    => Set<InventoryStock>();
    public DbSet<InventoryTransaction> InventoryTransactions
    => Set<InventoryTransaction>();
    public DbSet<InventoryCostLayer> InventoryCostLayers
    => Set<InventoryCostLayer>();
    protected override void OnModelCreating(
    ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Warehouse>(entity =>
        {
            entity.ToTable("warehouses");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(x => x.Address)
                .HasColumnType("nvarchar(max)");

            entity.HasIndex(x => x.Code)
                .IsUnique();
        });

        builder.Entity<InventoryStock>(entity =>
        {
            entity.ToTable("inventory_stock");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Quantity)
                .HasDefaultValue(0);

            entity.Property(x => x.ReservedQuantity)
                .HasDefaultValue(0);

            entity.HasIndex(x => new
            {
                x.WarehouseId,
                x.ProductVariantId
            }).IsUnique();
        });

        builder.Entity<InventoryTransaction>(entity =>
        {
            entity.ToTable("inventory_transactions");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.TransactionType)
                .HasConversion<string>();

            entity.Property(x => x.ReferenceType)
                .HasMaxLength(50);
            entity.Property(x => x.CreatedBy)
                .HasMaxLength(450);

            entity.Property(x => x.Note);
        });

        builder.Entity<InventoryCostLayer>(entity =>
        {
            entity.ToTable("inventory_cost_layers");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.ImportPrice)
                .HasColumnType("decimal(18,2)");
        });
    }
}