using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BikeManagerV3.Customer.Data;

public class InventoryDbContextFactory
    : IDesignTimeDbContextFactory<CustomerDbContext>
{
    public CustomerDbContext CreateDbContext(
        string[] args)
    {
        var optionsBuilder =
            new DbContextOptionsBuilder<CustomerDbContext>();

        optionsBuilder.UseSqlServer(
            "Server=localhost;Database=BikeManager_V3;Trusted_Connection=True;TrustServerCertificate=True");

        return new CustomerDbContext(
            optionsBuilder.Options);
    }
}