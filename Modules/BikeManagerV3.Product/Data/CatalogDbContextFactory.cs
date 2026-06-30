using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BikeManagerV3.Product.Data;

public class CatalogDbContextFactory
    : IDesignTimeDbContextFactory<CatalogDbContext>
{
    public CatalogDbContext CreateDbContext(
        string[] args)
    {
        var optionsBuilder =
            new DbContextOptionsBuilder<CatalogDbContext>();

        optionsBuilder.UseSqlServer(
            "Server=localhost;Database=BikeManager_V3;Trusted_Connection=True;TrustServerCertificate=True");

        return new CatalogDbContext(
            optionsBuilder.Options);
    }
}