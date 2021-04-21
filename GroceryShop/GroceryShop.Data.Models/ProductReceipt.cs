namespace GroceryShop.Data.Models
{
    public class ProductReceipt
    {
        public int ReceiptId { get; set; }

        public virtual Receipt Receipt { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}