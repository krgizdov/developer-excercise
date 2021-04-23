namespace GroceryShop.Services.Data
{
    using GroceryShop.Common;
    using GroceryShop.Data.Common.Repositories;
    using GroceryShop.Data.Models;
    using GroceryShop.Services.Mapping;
    using GroceryShop.Web.Infrastructure.Exceptions;
    using GroceryShop.Web.ViewModels.Receipts;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ReceiptsService : IReceiptsService
    {
        private readonly IDeletableEntityRepository<Receipt> receiptRepository;
        private readonly IProductsService productsService;
        private readonly IDealsService dealsService;

        public ReceiptsService(IDeletableEntityRepository<Receipt> receiptRepository,
            IProductsService productsService,
            IDealsService dealsService)
        {
            this.receiptRepository = receiptRepository;
            this.productsService = productsService;
            this.dealsService = dealsService;
        }
        public async Task<IEnumerable<T>> GetAllAsync<T>(int count = 5)
        {
            if (count <= 0 || count > 50)
            {
                throw new InvalidParameterException(GlobalConstants.InvalidQueryCount);
            }

            var receipts = await this.receiptRepository
                .AllAsNoTracking()
                .Take(count)
                .To<T>()
                .ToListAsync();

            return receipts;
        }

        public async Task<T> GetByIdAsync<T>(int id)
        {
            var receipt = await this.receiptRepository
                .AllAsNoTracking()
                .Where(r => r.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            if (receipt == null)
            {
                throw new ObjectNotFoundException(string.Format(GlobalConstants.ObjectWithIdNotFound, nameof(Receipt), id));
            }

            return receipt;
        }

        public async Task<ReceiptViewModel> CreateReceiptAsync(string[] scannedProducts)
        {
            if (scannedProducts.Length == 0)
            {
                throw new InvalidParameterException(GlobalConstants.InvalidProductAmount);
            }

            var receipt = new Receipt();
            
            var products = await this.GetProductsForPurchaseAsync(scannedProducts, receipt);
            var deals = await this.dealsService.GetAllAsync();

            var totalPrice = products.Sum(p => p.Price);
            var discountPrice = this.CalculateDiscountPriceAsync(products, deals, totalPrice);
            var discount = totalPrice - discountPrice;

            receipt.TotalPrice = totalPrice;
            receipt.TotalPriceWithDiscount = discountPrice;
            receipt.Discount = discount;

            await this.receiptRepository.AddAsync(receipt);
            await this.receiptRepository.SaveChangesAsync();

            return AutoMapperConfig.MapperInstance.Map<ReceiptViewModel>(receipt);
        }

        private decimal CalculateDiscountPriceAsync(
            IEnumerable<Product> products, IEnumerable<Deal> deals, decimal totalPrice)
        {
            var productsTemp = new List<Product>();

            totalPrice = CalculateTwoForThree(
                products.ToList(),
                productsTemp,
                deals.FirstOrDefault(d => d.Name == "2 for 3"),
                totalPrice);

            totalPrice = CalculateBuyOneGetOneHalfPrice(
                productsTemp,
                deals.FirstOrDefault(d => d.Name == "buy 1 get 1 half price"),
                totalPrice);

            return totalPrice;
        }

        private decimal CalculateBuyOneGetOneHalfPrice(List<Product> products, Deal deal, decimal totalPrice)
        {
            var validForDiscount = new Dictionary<string, List<Product>>();

            FindValidProductsCount(products, deal, validForDiscount);

            foreach (var kvp in validForDiscount)
            {
                while (kvp.Value.Count >= 2)
                {
                    totalPrice -= kvp.Value.FirstOrDefault().Price / 2;
                    kvp.Value.RemoveRange(0, 2);
                }
            }

            return totalPrice;
        }

        private void FindValidProductsCount(List<Product> products, Deal deal, 
            Dictionary<string, List<Product>> validForDiscount)
        {
            for (int i = 0; i < products.Count; i++)
            {
                var product = products[i];

                if (deal.Products.Any(pd => pd.Product.Name == product.Name))
                {
                    if (!validForDiscount.ContainsKey(product.Name))
                    {
                        validForDiscount.Add(product.Name, new List<Product>());
                    }

                    validForDiscount[product.Name].Add(product);
                }
            }
        }

        private decimal CalculateTwoForThree(
            List<Product> products, List<Product> tempProducts, Deal deal, decimal totalPrice)
        {
            var validForDiscount = new Dictionary<string, Product>();
            int counter = 0;

            for (int i = 0; i < products.Count; i++)
            {
                var product = products[i];

                if (deal.Products.Any(pd => pd.Product.Name == product.Name))
                {
                    if (!validForDiscount.ContainsKey(product.Name))
                    {
                        validForDiscount.Add(product.Name, product);
                    }
                    counter++;
                }
                else
                {
                    tempProducts.Add(product);
                }

                if (counter == 3)
                {
                    totalPrice -= validForDiscount.Values.Min(v => v.Price);
                    validForDiscount.Clear();
                    counter = 0;
                }
            }

            if (validForDiscount.Count > 0)
            {
                tempProducts.AddRange(validForDiscount.Values);
            }

            return totalPrice;
        }

        private async Task<IEnumerable<Product>> GetProductsForPurchaseAsync(string[] scannedProducts, Receipt receipt)
        {
            var products = new List<Product>();
            var usedProducts = new HashSet<string>();

            foreach (var productName in scannedProducts)
            {
                var product = await this.CheckIfProductCanBePurchasedAsync(productName);

                products.Add(product);

                if (!usedProducts.Contains(productName))
                {
                    receipt.Products.Add(new ProductReceipt
                    {
                        Product = product,
                        Receipt = receipt
                    });

                    usedProducts.Add(productName);
                }
            }

            return products;
        }

        private async Task<Product> CheckIfProductCanBePurchasedAsync(string productName)
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

            return product;
        }
    }
}
