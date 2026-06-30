using BikeManagerV3.Order.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Order.Data;

public class OrderDbContext : DbContext
{
    public OrderDbContext(
        DbContextOptions<OrderDbContext> options)
        : base(options)
    {
    }

    public DbSet<Models.Order> Orders
        => Set<Models.Order>();

    public DbSet<OrderItem> OrderItems
        => Set<OrderItem>();

    public DbSet<InstallmentProvider> InstallmentProviders
        => Set<InstallmentProvider>();

    public DbSet<InstallmentContract> InstallmentContracts
        => Set<InstallmentContract>();

    protected override void OnModelCreating(
        ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // orders
        builder.Entity<Models.Order>(entity =>
        {
            entity.ToTable("orders");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.OrderCode)
                .HasMaxLength(100);

            entity.Property(x => x.SubTotal)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.DiscountAmount)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.TaxAmount)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.TotalAmount)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.PaymentMethod)
                .HasMaxLength(50);

            entity.Property(x => x.PaymentStatus)
                .HasMaxLength(50);

            entity.Property(x => x.OrderStatus)
                .HasMaxLength(50);

            entity.HasMany(x => x.Items)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId);

            entity.HasMany(x => x.InstallmentContracts)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId);
        });

        // order_items
        builder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("order_items");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.UnitPrice)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.DiscountAmount)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.TotalPrice)
                .HasColumnType("decimal(18,2)");
            entity.HasOne(x => x.Order)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.OrderId);
        });

        // installment_providers
        builder.Entity<InstallmentProvider>(entity =>
        {
            entity.ToTable("installment_providers");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasMaxLength(255);

            entity.Property(x => x.Phone)
                .HasMaxLength(20);

            entity.Property(x => x.ApiEndpoint)
                .HasColumnType("nvarchar(max)");
        });

        // installment_contracts
        builder.Entity<InstallmentContract>(entity =>
        {
            entity.ToTable("installment_contracts");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.ContractNumber)
                .HasMaxLength(255);

            entity.Property(x => x.LoanAmount)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.DownPayment)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.MonthlyPayment)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.InterestRate)
                .HasColumnType("decimal(5,2)");

            entity.Property(x => x.ContractStatus)
                .HasMaxLength(50);

            entity.HasOne(x => x.Provider)
                .WithMany(x => x.Contracts)
                .HasForeignKey(x => x.ProviderId);
        });
    }
}