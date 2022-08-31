using System.IO;
using AuthenticationProject.Data.Maps.StoreDataContext;
using AuthenticationProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AuthenticationProject.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<StoreDataContext>
{
    StoreDataContext IDesignTimeDbContextFactory<StoreDataContext>.CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<StoreDataContext>();
        builder.UseSqlServer(@"Server=.;Database=StoreDB;User ID=sa;Password=k2n3a9");
        return new StoreDataContext(builder.Options);
    }
}

public class StoreDataContext : IdentityDbContext<User>
{
    public StoreDataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("AppDatabase");
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new ProductMap());
        builder.ApplyConfiguration(new CategoryMap());

        SeedUsers(builder);
        SeedRoles(builder);
        SeedUserRoles(builder);
    }

    private static void SeedUsers(ModelBuilder builder)
    {
        var user = new User()
        {
            Id = "b74ddd14-6340-4840-95c2-db12554843e5",
            UserName = "admin",
            NormalizedUserName = "admin",
            Email = "kaique.11@hotmail.com",
            NormalizedEmail = "kaique.11@hotmail.com",
            EmailConfirmed = true,
            FirsName = "System",
            LastName = "Admin",
            PhoneNumberConfirmed = true,
            LockoutEnabled = false,
            PhoneNumber = "00000000",
            TwoFactorEnabled = false
        };

        var passwordHasher = new PasswordHasher<User>();
        user.PasswordHash = passwordHasher.HashPassword(user, "Admin*123");

        builder.Entity<User>().HasData(user);
    }

    private static void SeedRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole()
            {
                Id = "fab4fac1-c546-41de-aebc-a14da6895711",
                Name = Extensions.UserRoles.Admin,
                ConcurrencyStamp = "1",
                NormalizedName = Extensions.UserRoles.Admin

            },
            new IdentityRole()
            {
                Id = "c7b013f0-5201-4317-abd8-c211f91b7330",
                Name = Extensions.UserRoles.Manager,
                ConcurrencyStamp = "2",
                NormalizedName = Extensions.UserRoles.Manager
            },
            new IdentityRole()
            {
                Id = "d27475f9-2aa6-41e3-b72b-ec9faed506f2",
                Name = Extensions.UserRoles.Employee,
                ConcurrencyStamp = "3",
                NormalizedName = Extensions.UserRoles.Employee
            }
            );
    }

    private static void SeedUserRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>()
            {
                RoleId = "fab4fac1-c546-41de-aebc-a14da6895711",
                UserId = "b74ddd14-6340-4840-95c2-db12554843e5"
            }
            );
    }
}
