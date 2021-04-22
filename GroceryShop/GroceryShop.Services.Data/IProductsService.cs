namespace GroceryShop.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProductsService
    {
        Task<IEnumerable<T>> GetAllAsync<T>(int? count = null);

        Task<T> GetByIdAsync<T>(int id);

        Task<int> CreateAsync(string name, decimal price);

        Task UpdateAsync(int id, string name, decimal price);

        Task DeleteAsync(int id);
    }
}
