namespace GroceryShop.Services.Data
{
    using GroceryShop.Data.Common.Repositories;
    using GroceryShop.Data.Models;
    using GroceryShop.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductsService : IProductsService
    {
        private readonly IDeletableEntityRepository<Product> productsRepository;

        public ProductsService(IDeletableEntityRepository<Product> productsRepository)
        {
            this.productsRepository = productsRepository;
        }

        public async Task<int> CreateAsync(string name, decimal price)
        {
            var product = new Product
            {
                Name = name,
                Price = price,
            };

            await this.productsRepository.AddAsync(product);
            await this.productsRepository.SaveChangesAsync();

            return product.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var product = await this.GetProductAsync(id);

            if (product == null)
            {
                return;
            }

            this.productsRepository.Delete(product);
            await this.productsRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(int? count = null)
        {
            //Get all products ordered by name. Optionally you can get a specified number of products.

            IQueryable<Product> query =
                this.productsRepository.AllAsNoTracking().OrderBy(c => c.Name);

            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            return await query.To<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync<T>(int id)
        {
            var product = await this.productsRepository
                .All()
                .Where(p => p.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            return product;
        }

        public async Task UpdateAsync(int id, string name, decimal price)
        {
            var product = await this.GetProductAsync(id);

            if (product == null)
            {
                return;
            }

            product.Name = name;
            product.Price = price;

            this.productsRepository.Update(product);
            await this.productsRepository.SaveChangesAsync();
        }

        private async Task<Product> GetProductAsync(int id)
        {
            var product = await this.productsRepository
                .All()
                .FirstOrDefaultAsync(p => p.Id == id);

            return product;
        }
    }
}
