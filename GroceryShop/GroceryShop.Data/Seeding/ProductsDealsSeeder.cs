namespace GroceryShop.Data.Seeding
{
    using GroceryShop.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    internal class ProductsDealsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            if (await dbContext.ProductDeals.AnyAsync())
            {
                return;
            }

            var deal = await dbContext.Deals.FirstOrDefaultAsync();
            var product = await dbContext.Products.FirstOrDefaultAsync();

            if (deal != null && product != null)
            {
                await dbContext.ProductDeals.AddAsync(new ProductDeal
                { 
                    Deal = deal,
                    Product = product,
                });
            }
        }
    }
}
