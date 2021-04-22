namespace GroceryShop.Web.ViewModels.Products
{
    using System.ComponentModel.DataAnnotations;

    public class ProductUpdateModel
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
