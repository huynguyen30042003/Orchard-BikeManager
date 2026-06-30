using BikeManagerV3.Inventory.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BikeManagerV3.Inventory.Data;

public class InventoryDbContextFactory
    : IDesignTimeDbContextFactory<InventoryDbContext>
{
    public InventoryDbContext CreateDbContext(
        string[] args)
    {
        var optionsBuilder =
            new DbContextOptionsBuilder<InventoryDbContext>();

        optionsBuilder.UseSqlServer(
            "Server=localhost;Database=BikeManager_V3;Trusted_Connection=True;TrustServerCertificate=True");

        return new InventoryDbContext(
            optionsBuilder.Options);
    }
}