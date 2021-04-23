namespace GroceryShop.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ApplicationDbContextSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            CheckForNullParameters(dbContext);

            var seeders = new List<ISeeder>
                          {
                              //new ProductsSeeder(),
                              new DealsSeeder(),
                              //new ProductsDealsSeeder(),
                          };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(dbContext);

                await dbContext.SaveChangesAsync();
            }
        }

        private static void CheckForNullParameters(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
        }
    }
}
