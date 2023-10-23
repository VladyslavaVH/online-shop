using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Text;

namespace OnlineShop.Models
{
    public class ShopDbContext : DbContext
    {
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<Role> Roles { get; set; }

        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Buyer>()
                    .HasMany(b => b.Products)
                    .WithMany(p => p.Buyers)
                    .UsingEntity<Order>(
                        j => j
                            .HasOne(o => o.Product)
                            .WithMany(p => p.Orders)
                            .HasForeignKey(o => o.ProductFK),
                        j => j
                            .HasOne(o => o.Buyer)
                            .WithMany(b => b.Orders)
                            .HasForeignKey(o => o.BuyerFK),
                        j =>
                        {
                            j.HasKey( k => new { k.ProductFK, k.BuyerFK });
                        }
                    );

            var password = "admin";
            var sha256 = new SHA256Managed();
            var passwordHash = Convert.ToBase64String(
                sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Buyer" },
                new Role { Id = 2, Name = "admin" }            
            );

            modelBuilder.Entity<Buyer>().HasData(
                new Buyer
                {
                    Id = 1,
                    FirstName = "Vladislava",
                    LastName = "Golovan",
                    Phone = "+380965323364",
                    Email = "admin@gmail.com",
                    Password = passwordHash,
                    RoleFK = 2
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
