namespace GroceryShop.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDealsService
    {
        Task<IEnumerable<T>> GetAllAsync<T>(int count = 5);

        Task<T> GetByIdAsync<T>(int id);

        Task<T> AddProductsToDealAsync<T>(int id, string[] productNames);
    }
}
