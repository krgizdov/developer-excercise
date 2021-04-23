namespace GroceryShop.Services.Data
{
    using GroceryShop.Common;
    using GroceryShop.Data.Common.Repositories;
    using GroceryShop.Data.Models;
    using GroceryShop.Services.Mapping;
    using GroceryShop.Web.Infrastructure.Exceptions;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DealsService : IDealsService
    {
        private readonly IDeletableEntityRepository<Deal> dealRepository;
        private readonly IProductsService productsService;

        public DealsService(IDeletableEntityRepository<Deal> dealRepository,
            IProductsService productsService)
        {
            this.dealRepository = dealRepository;
            this.productsService = productsService;
        }

        public async Task<T> AddProductsToDealAsync<T>(int id, string[] productNames)
        {
            if (productNames.Length == 0)
            {
                throw new InvalidParameterException(GlobalConstants.InvalidProductAmount);
            }

            var deal = await this.GetDealAsync(id);

            foreach (var productName in productNames)
            {
                var product = await this.CheckIfProductCanBeAddedAsync(productName, deal);

                deal.Products.Add(new ProductDeal
                {
                    Deal = deal,
                    Product = product
                });
            }

            this.dealRepository.Update(deal);

            await this.dealRepository.SaveChangesAsync();

            return await this.GetByIdAsync<T>(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(int count = 5)
        {
            if (count <= 0 || count > 50)
            {
                throw new InvalidParameterException(GlobalConstants.InvalidQueryCount);
            }

            var deals =  await this.dealRepository
                .AllAsNoTracking()
                .Take(count)
                .To<T>()
                .ToListAsync();

            return deals;
        }

        public async Task<IEnumerable<Deal>> GetAllAsync(int count = 5)
        {
            if (count <= 0 || count > 50)
            {
                throw new InvalidParameterException(GlobalConstants.InvalidQueryCount);
            }

            var deals = await this.dealRepository
                .AllAsNoTracking()
                .Include(d => d.Products)
                .ThenInclude(pd => pd.Product)
                .Take(count)
                .ToListAsync();

            return deals;
        }

        public async Task<T> GetByIdAsync<T>(int id)
        {
            var deal = await this.dealRepository
                .AllAsNoTracking()
                .Where(d => d.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            if (deal == null)
            {
                throw new ObjectNotFoundException(string.Format(GlobalConstants.ObjectWithIdNotFound, nameof(Deal), id));
            }

            return deal;
        }

        private async Task<Product> CheckIfProductCanBeAddedAsync(string productName, Deal deal)
        {
            if (string.IsNullOrEmpty(productName) || productName.Length > 50)
            {
                throw new InvalidParameterException(GlobalConstants.InvalidProductName);
            }

            var product = await productsService.GetByNameAsync(productName);

            if (product == null)
            {
                throw new ObjectNotFoundException(string.Format(GlobalConstants.ProductNotFound, productName));
            }

            if (deal.Products.Any(p => p.Product.Name == productName))
            {
                throw new ObjectExistsException(string.Format(GlobalConstants.ProductAlreadyInDeal, productName));
            }

            return product;
        }

        private async Task<Deal> GetDealAsync(int id)
        {
            var deal = await this.dealRepository
                .All()
                .Include(d => d.Products)
                .ThenInclude(pd => pd.Product)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (deal == null)
            {
                throw new ObjectNotFoundException(string.Format(GlobalConstants.ObjectWithIdNotFound, nameof(Deal), id));
            }

            return deal;
        }
    }
}
