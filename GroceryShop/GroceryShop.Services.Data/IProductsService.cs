namespace GroceryShop.Services.Data
{
    using GroceryShop.Data.Models;
    using GroceryShop.Web.ViewModels.Products;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProductsService
    {
        Task<IEnumerable<T>> GetAllAsync<T>(int count);

        Task<T> GetByIdAsync<T>(int id);

        Task<IEnumerable<ProductViewModel>> CreateAsync(ProductCreateInputModel[] inputModels);

        Task UpdateAsync(int id, string name, decimal price);

        Task DeleteAsync(int id);

        Task<Product> GetByNameAsync(string name);
    }
}
