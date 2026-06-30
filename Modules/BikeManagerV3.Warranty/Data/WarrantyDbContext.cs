using BikeManagerV3.Warranty.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Warranty.Data;

public class WarrantyDbContext : DbContext
{
    public WarrantyDbContext(
        DbContextOptions<WarrantyDbContext> options)
        : base(options)
    {
    }

    public DbSet<WarrantyModel> Warranties => Set<WarrantyModel>();

    public DbSet<WarrantyClaim> WarrantyClaims => Set<WarrantyClaim>();
    protected override void OnModelCreating(
        ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<WarrantyModel>(entity =>
        {
            entity.ToTable("warranties");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Status)
                .HasMaxLength(50);

            entity.HasMany(x => x.WarrantyClaims)
                .WithOne(x => x.Warranty)
                .HasForeignKey(x => x.WarrantyId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<WarrantyClaim>(entity =>
        {
            entity.ToTable("warranty_claims");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Status)
                .HasMaxLength(50);

            entity.Property(x => x.IssueDescription)
                .HasColumnType("nvarchar(max)");

            entity.Property(x => x.Resolution)
                .HasColumnType("nvarchar(max)");
        });
    }
}