using ECommerceMicroservice.Core.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMicroservice.Infrastructure.Data
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context, ILogger<ApplicationDbContext> logger)
        {
            if (!context.Categories.Any())
            {
                var categories = new[]
                {
                    new Category { Name = "Elektronik" },
                    new Category { Name = "Kitap" },
                    new Category { Name = "Ev & Yaşam" }
                };
                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
                logger.LogInformation("Kategoriler başarıyla eklendi.");
            }

            if (!context.Products.Any())
            {
                var products = new[]
                {
                    new Product { Name = "Laptop", Price = 15000, Stock = 50, CategoryId = context.Categories.FirstOrDefault(c => c.Name == "Elektronik").Id },
                    new Product { Name = "Akıllı Telefon", Price = 8000, Stock = 100, CategoryId = context.Categories.FirstOrDefault(c => c.Name == "Elektronik").Id },
                    new Product { Name = "Dostoyevski - Suç ve Ceza", Price = 50, Stock = 200, CategoryId = context.Categories.FirstOrDefault(c => c.Name == "Kitap").Id },
                    new Product { Name = "Süpürge", Price = 1200, Stock = 30, CategoryId = context.Categories.FirstOrDefault(c => c.Name == "Ev & Yaşam").Id }
                };
                context.Products.AddRange(products);
                await context.SaveChangesAsync();
                logger.LogInformation("Ürünler başarıyla eklendi.");
            }
        }
    }
}
