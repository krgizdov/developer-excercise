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
                new Product{ Name = "apple", Price = 50, },
                new Product{ Name = "banana", Price = 40, },
                new Product{ Name = "tomato", Price = 30, },
                new Product{ Name = "potato", Price = 26, }
            };

            foreach (var product in products)
            {
                await dbContext.Products.AddAsync(product);
            }
        }
    }
}
