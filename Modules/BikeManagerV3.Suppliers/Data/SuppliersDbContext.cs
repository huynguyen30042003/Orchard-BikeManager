using BikeManagerV3.Suppliers.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Suppliers.Data;

public class SuppliersDbContext : DbContext
{
    public SuppliersDbContext(
        DbContextOptions<SuppliersDbContext> options)
        : base(options)
    {
    }

    public DbSet<Supplier> Suppliers
=> Set<Supplier>();

    public DbSet<PurchaseOrder> PurchaseOrders
=> Set<PurchaseOrder>();

    public DbSet<PurchaseOrderItem> PurchaseOrderItems
=> Set<PurchaseOrderItem>();

    protected override void OnModelCreating(
        ModelBuilder builder)
    {
        base.OnModelCreating(builder);


        builder.Entity<Supplier>(entity =>
        {
            entity.ToTable("suppliers");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Code)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.Name)
                .HasMaxLength(255)
                .IsRequired();

            entity.HasIndex(x => x.Code)
                .IsUnique();
        });

        builder.Entity<PurchaseOrder>(entity =>
        {
            entity.ToTable("purchase_orders");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Code)
                .HasMaxLength(50);

            entity.Property(x => x.SubTotal)
                .HasPrecision(18, 2);

            entity.Property(x => x.TotalAmount)
                .HasPrecision(18, 2);

            entity.Property(x => x.DiscountAmount)
                .HasPrecision(18, 2);

            entity.HasOne(x => x.Supplier)
                .WithMany(x => x.PurchaseOrders)
                .HasForeignKey(x => x.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<PurchaseOrderItem>(entity =>
        {
            entity.ToTable("purchase_order_items");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.UnitPrice)
                .HasPrecision(18, 2);

            entity.Property(x => x.DiscountAmount)
                .HasPrecision(18, 2);

            entity.Property(x => x.TotalAmount)
                .HasPrecision(18, 2);

            entity.HasOne(x => x.PurchaseOrder)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.PurchaseOrderId);
        });
    }
}