namespace GroceryShop.Services.Data
{
    using GroceryShop.Common;
    using GroceryShop.Data.Common.Repositories;
    using GroceryShop.Data.Models;
    using GroceryShop.Services.Mapping;
    using GroceryShop.Web.Infrastructure.Exceptions;
    using GroceryShop.Web.ViewModels.Products;
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

        public async Task<IEnumerable<ProductViewModel>> CreateAsync(ProductCreateInputModel[] inputModels)
        {
            if (inputModels.Length == 0)
            {
                throw new InvalidParameterException(GlobalConstants.InvalidProductAmount);
            }

            var products = new List<Product>();

            foreach (var input in inputModels)
            {
                await this.CheckIfProductCanBeAddedOrUpdatedAsync(input.Name, input.Price);

                var product = new Product
                {
                    Name = input.Name,
                    Price = input.Price,
                };

                await this.productsRepository.AddAsync(product);
                products.Add(product);
            }

            await this.productsRepository.SaveChangesAsync();

            var productViewModels = AutoMapperConfig
                .MapperInstance.
                Map<IEnumerable<ProductViewModel>>(products);

            return productViewModels;
        }

        public async Task DeleteAsync(int id)
        {
            var product = await this.GetProductAsync(id);

            this.productsRepository.Delete(product);
            await this.productsRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(int count)
        {
            if (count <= 0 || count > 50)
            {
                throw new InvalidParameterException(GlobalConstants.InvalidQueryCount);
            }

            //Get all products ordered by name. Optionally you can get a specified number of products.

            var products = await this.productsRepository
                .AllAsNoTracking()
                .OrderBy(c => c.Name)
                .Take(count)
                .To<T>()
                .ToListAsync();

            return products;
        }

        public async Task<T> GetByIdAsync<T>(int id)
        {
            var product = await this.productsRepository
                .AllAsNoTracking()
                .Where(p => p.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            if (product == null)
            {
                throw new ObjectNotFoundException(string.Format(GlobalConstants.ObjectWithIdNotFound, nameof(Product), id));
            }

            return product;
        }

        public async Task UpdateAsync(int id, string name, decimal price)
        {
            var product = await this.GetProductAsync(id);

            await this.CheckIfProductCanBeAddedOrUpdatedAsync(name, price);

            product.Name = name;
            product.Price = price;

            this.productsRepository.Update(product);
            await this.productsRepository.SaveChangesAsync();
        }

        public async Task<Product> GetByNameAsync(string name)
        {
            var product = await this.productsRepository
                .All()
                .FirstOrDefaultAsync(p => p.Name == name);

            return product;
        }

        private async Task<Product> GetProductAsync(int id)
        {
            var product = await this.productsRepository
                .All()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                throw new ObjectNotFoundException(string.Format(GlobalConstants.ObjectWithIdNotFound, nameof(Product), id));
            }

            return product;
        }

        private async Task CheckIfProductCanBeAddedOrUpdatedAsync(string name, decimal price)
        {
            if (string.IsNullOrEmpty(name) || name.Length > 50)
            {
                throw new InvalidParameterException(GlobalConstants.InvalidProductName);
            }

            if (price < 0)
            {
                throw new InvalidParameterException(GlobalConstants.InvalidProductPrice);
            }

            var product = await this.GetByNameAsync(name);

            if (product != null)
            {
                throw new ObjectExistsException(string.Format(GlobalConstants.ProductAlreadyExists, name));
            }
        }
    }
}
