namespace GroceryShop.Data.Seeding
{
    using GroceryShop.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    internal class DealsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            if (await dbContext.Deals.AnyAsync())
            {
                return;
            }

            var deals = new List<Deal>
            {
                new Deal{ Name = "2 for 3" },
                new Deal{ Name = "buy 1 get 1 half price" }
            };

            foreach (var deal in deals)
            {
                await dbContext.Deals.AddAsync(deal);
            }
        }
    }
}
