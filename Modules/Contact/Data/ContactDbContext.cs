using Contact.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Contact.Data;

public class ContactDbContext : DbContext
{
    public ContactDbContext(
        DbContextOptions<ContactDbContext> options)
        : base(options)
    {
    }

    public DbSet<ContactModel> Contacts => Set<ContactModel>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ContactModel>(entity =>
        {
            entity.ToTable("contacts");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.FullName)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(x => x.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(x => x.Email)
                .HasMaxLength(255);

            entity.Property(x => x.Title)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(x => x.Content)
                .IsRequired();
        });
    }
}
