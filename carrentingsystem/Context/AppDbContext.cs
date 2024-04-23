using carrentingsystem.Models;
using Microsoft.EntityFrameworkCore;

namespace carrentingsystem.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed Admin User
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Role= RoleType.Admin,
                    Email = "admin@example.com",
                    Password = "adminpassword",
                    Name = "Admin",
                    IsAdmin = true
                }
            );
        }
    }
    }

