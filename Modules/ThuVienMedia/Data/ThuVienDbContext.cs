using Microsoft.EntityFrameworkCore;
using ThuVienMedia.Models;
namespace ThuVienMedia.Data;

public class ThuVienDbContext : DbContext
{
    public ThuVienDbContext(
        DbContextOptions<ThuVienDbContext> options)
        : base(options)
    {
    }

    public DbSet<ThuVienMediaModel> ThuVienMedia
        => Set<ThuVienMediaModel>();
    public DbSet<CMThuVienMedia> CMThuVienMedia
        => Set<CMThuVienMedia>();
    public DbSet<FileMedia> FileMedia
        => Set<FileMedia>();

    protected override void OnModelCreating(
        ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<CMThuVienMedia>(entity =>
        {
            entity.ToTable("CMThuVienMedia");

            entity.HasKey(x => x.ID);

            entity.Property(x => x.OwnerCode)
                .HasMaxLength(40)
                .IsRequired();

            entity.Property(x => x.TenChuyenMuc)
                .HasMaxLength(100);

            entity.Property(x => x.IDChuyenMucCapCha)
                .HasMaxLength(40);
        });

        builder.Entity<ThuVienMediaModel>(entity =>
        {
            entity.ToTable("ThuVienMedia");

            entity.HasKey(x => x.ID);

            entity.Property(x => x.OwnerCode)
                .HasMaxLength(40)
                .IsRequired();

            entity.Property(x => x.TenThuVien)
                .HasMaxLength(100);

            entity.Property(x => x.MoTa)
                .HasMaxLength(1000);

            entity.Property(x => x.GioiThieu)
                .HasMaxLength(1000);

            entity.Property(x => x.UrlThumbAnhDaiDien)
                .HasMaxLength(500);

            entity.Property(x => x.UrlAnhDaiDien)
                .HasMaxLength(500);

            entity.Property(x => x.IDFileMediaList)
                .HasColumnType("varchar(MAX)");
            entity.HasOne(x => x.ChuyenMucMedia)
                .WithMany(x => x.ThuVienMedias)
                .HasForeignKey(x => x.IDChuyenMucMedia)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<FileMedia>(entity =>
        {
            entity.ToTable("FileMedia");

            entity.HasKey(x => x.ID);

            entity.Property(x => x.OwnerCode)
                .HasMaxLength(40)
                .IsRequired();

            entity.Property(x => x.TenFile)
                .HasMaxLength(100);

            entity.Property(x => x.TieuDe)
                .HasMaxLength(100);

            entity.Property(x => x.TacGia)
                .HasMaxLength(100);

            entity.Property(x => x.UrlFile)
                .HasMaxLength(500);

            entity.Property(x => x.UrlThumb)
                .HasMaxLength(500);

            entity.Property(x => x.UrlAnhDaiDienVideo)
                .HasMaxLength(500);

            entity.Property(x => x.MoTa)
                .HasColumnType("nvarchar(MAX)");

            entity.HasOne(x => x.ThuVienMedia)
                .WithMany(x => x.Files)
                .HasForeignKey(x => x.IDThuVienMedia)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}