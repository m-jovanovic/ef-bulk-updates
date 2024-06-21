using BulkInserts;
using System;
using System.Linq;
using BulkInserts.Entities;
using Microsoft.EntityFrameworkCore;

using var context = new ApplicationDbContext();
using var transaction = context.Database.BeginTransaction();

try
{
   var product = context.Products.First(p => p.Id == 1);
   product.Price = 99.99m;

   var newProduct = new Product { Name = "NewGadget", Price = 129.99m };
   context.Products.Add(newProduct);

   await context.Products
       .Where(p => p.Category == "Electronics")
       .ExecuteUpdateAsync(s =>
           s.SetProperty(p => p.Price, p => p.Price + 0.01m));

   await context.SaveChangesAsync();

   transaction.Commit();
}
catch (Exception ex)
{
   transaction.Rollback();
}

Console.ReadKey();
