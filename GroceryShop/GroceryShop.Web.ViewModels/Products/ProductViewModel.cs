namespace GroceryShop.Web.ViewModels.Products
{
    using GroceryShop.Data.Models;
    using GroceryShop.Services.Mapping;

    public class ProductViewModel : IMapFrom<Product>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}
