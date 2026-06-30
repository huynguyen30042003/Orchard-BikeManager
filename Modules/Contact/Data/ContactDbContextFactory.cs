using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BikeManagerV3.Contact.Data;

public class ContactDbContextFactory
    : IDesignTimeDbContextFactory<ContactDbContext>
{
    public ContactDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder =
            new DbContextOptionsBuilder<ContactDbContext>();

        optionsBuilder.UseSqlServer(
            "Server=localhost;Database=BikeManager_V3;Trusted_Connection=True;TrustServerCertificate=True");

        return new ContactDbContext(optionsBuilder.Options);
    }
}
