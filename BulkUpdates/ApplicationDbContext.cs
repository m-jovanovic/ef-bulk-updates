using System;
using System.Linq;
using Bogus;
using BulkInserts.Entities;
using Microsoft.EntityFrameworkCore;

namespace BulkInserts;

public class ApplicationDbContext : DbContext
{
    public const string ConnectionString =
        "Server=localhost;Database=EF.Performance;Trusted_Connection=True;Integrated Security=true;MultipleActiveResultSets=true;TrustServerCertificate=true";

    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(ConnectionString);//.LogTo(Console.WriteLine);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasKey(p => p.Id);

        modelBuilder.Entity<Product>().Property(p => p.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(38, 0);

        var faker = new Faker();
        var products = Enumerable.Range(1, 10_000)
            .Select(id => new Product
            {
                Id = id,
                Name = faker.Commerce.ProductName(),
                Description = faker.Commerce.ProductDescription(),
                Category = "Electronics",
                Price = faker.Random.Decimal(1, 100)
            });

        modelBuilder.Entity<Product>().HasData(products);
    }
}