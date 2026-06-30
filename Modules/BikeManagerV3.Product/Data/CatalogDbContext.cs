using BikeManagerV3.Product.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Product.Data;

public class CatalogDbContext : DbContext
{
    public CatalogDbContext(
        DbContextOptions<CatalogDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Brand> Brands => Set<Brand>();

    public DbSet<ProductModels> Products => Set<ProductModels>();

    public DbSet<ProductVariant> ProductVariants
        => Set<ProductVariant>();

    public DbSet<ProductImage> ProductImages
        => Set<ProductImage>();

    public DbSet<SerialNumber> SerialNumbers
        => Set<SerialNumber>();
    public DbSet<Counter> Counters { get; set; }
    protected override void OnModelCreating(
        ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // CATEGORY
        builder.Entity<Category>(entity =>
        {
            entity.ToTable("categories");

            entity.HasKey(x => x.Id);

            entity.HasIndex(x => x.Slug)
                .IsUnique();

            entity.HasOne(x => x.Parent)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // BRAND
        builder.Entity<Brand>(entity =>
        {
            entity.ToTable("brands");

            entity.HasKey(x => x.Id);

            entity.HasIndex(x => x.Slug)
                .IsUnique();
        });

        // PRODUCT
        builder.Entity<ProductModels>(entity =>
        {
            entity.ToTable("products");

            entity.HasKey(x => x.Id);

            entity.HasIndex(x => x.SKU)
                .IsUnique();

            entity.HasIndex(x => x.Slug)
                .IsUnique();

            entity.HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId);

            entity.HasOne(x => x.Brand)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.BrandId);
        });

        // VARIANT
        builder.Entity<ProductVariant>(entity =>
        {
            entity.ToTable("product_variants");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.SKU)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Color)
                .HasMaxLength(50);

            entity.Property(x => x.Battery)
                .HasMaxLength(100);

            entity.Property(x => x.MotorPower)
                .HasMaxLength(100);

            entity.Property(x => x.ImportPrice)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.SellingPrice)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.WholesalePrice)
                .HasColumnType("decimal(18,2)");

            entity.HasIndex(x => x.SKU)
                .IsUnique();

            entity.HasOne(x => x.Product)
                .WithMany(x => x.ProductVariants)
                .HasForeignKey(x => x.ProductId);
        });

        // IMAGES
        builder.Entity<ProductImage>(entity =>
        {
            entity.ToTable("product_images");

            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.Product)
                .WithMany(x => x.Images)
                .HasForeignKey(x => x.ProductId);
        });

        builder.Entity<SerialNumber>(entity =>
        {
            entity.ToTable("serial_numbers");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.SerialCode)
                .HasMaxLength(200)
                .IsRequired();

            entity.HasIndex(x => x.SerialCode)
                .IsUnique();
            entity.Property(x => x.CurrentStatus)
                .HasConversion<string>();
            entity.HasOne(x => x.ProductVariant)
                .WithMany(x => x.SerialNumbers)
                .HasForeignKey(x => x.ProductVariantId);
        });
    }
}