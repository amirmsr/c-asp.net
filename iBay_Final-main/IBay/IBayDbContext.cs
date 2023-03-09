using System;
using IBay.Models;
using Microsoft.EntityFrameworkCore;
namespace IBay
{
	public class IBayDbContext : DbContext
	{
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }

        public IBayDbContext(DbContextOptions<IBayDbContext> options) : base(options)
		{
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Email = "iyad@example.com", Pseudo = "iyad", Password = "iyad", Role = "admin" },
                new User { Id = 2, Email = "seller@example.com", Pseudo = "seller", Password = "password", Role = "seller" },
                new User { Id = 3, Email = "amir@example.com", Pseudo = "amir", Password = "password", Role = "seller" },
                new User { Id = 4, Email = "user@example.com", Pseudo = "user", Password = "password", Role = "user" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "iPhone 14", Image = "image1.jpg", Price = 1399.99, Available = true, AddedTime = DateTime.UtcNow, SellerId = 2 },
                new Product { Id = 2, Name = "Macbook Pro M1", Image = "image2.jpg", Price = 2399.99, Available = true, AddedTime = DateTime.UtcNow, SellerId = 2 },
                new Product { Id = 3, Name = "iPad Air", Image = "image3.jpg", Price = 599.99, Available = true, AddedTime = DateTime.UtcNow, SellerId = 1 },
                new Product { Id = 4, Name = "Dyson 3000", Image = "image4.jpg", Price = 155.99, Available = false, AddedTime = DateTime.UtcNow, SellerId = 3 },
                new Product { Id = 5, Name = "Un Rein", Image = "image5.jpg", Price = 1000000.99, Available = true, AddedTime = DateTime.UtcNow, SellerId = 2 },
                new Product { Id = 6, Name = "Ma petite soeur", Image = "image6.jpg", Price = 0.01, Available = true, AddedTime = DateTime.UtcNow, SellerId = 3 }
            );
        }
    }
}

