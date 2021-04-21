namespace GroceryShop.Data.Seeding
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using GroceryShop.Data.Models;
    using Microsoft.EntityFrameworkCore;

    internal class ProductsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            if (await dbContext.Products.AnyAsync())
            {
                return;
            }

            var products = new List<Product>
            {
                new Product{ Name = "apple", Price = 0.50m, },
                new Product{ Name = "banana", Price = 0.40m, },
                new Product{ Name = "tomato", Price = 0.30m, },
                new Product{ Name = "potato", Price = 0.26m, }
            };

            foreach (var product in products)
            {
                await dbContext.Products.AddAsync(product);
            }
        }
    }
}
