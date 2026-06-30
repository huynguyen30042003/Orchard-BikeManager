using BikeManagerV3.Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeManagerV3.Auth.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(
            DbContextOptions<UserDbContext> options
        ) : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
    }
}