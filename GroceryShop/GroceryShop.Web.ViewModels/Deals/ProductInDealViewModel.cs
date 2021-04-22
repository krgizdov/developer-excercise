namespace GroceryShop.Web.ViewModels.Deals
{
    using GroceryShop.Data.Models;
    using GroceryShop.Services.Mapping;

    public class ProductInDealViewModel : IMapFrom<Product>
    {
        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}