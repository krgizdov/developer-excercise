namespace GroceryShop.Services.Data
{
    using GroceryShop.Web.ViewModels.Receipts;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IReceiptsService
    {
        Task<IEnumerable<T>> GetAllAsync<T>(int count = 5);

        Task<T> GetByIdAsync<T>(int id);

        Task<ReceiptViewModel> CreateReceiptAsync(string[] scannedProducts);
    }
}
