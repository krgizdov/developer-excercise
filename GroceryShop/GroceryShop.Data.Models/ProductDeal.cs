namespace GroceryShop.Data.Models
{
    public class ProductDeal
    {
        public int DealId { get; set; }

        public virtual Deal Deal { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}