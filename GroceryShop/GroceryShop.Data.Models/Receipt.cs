namespace GroceryShop.Data.Models
{
    using GroceryShop.Data.Common.Models;
    using System.Collections.Generic;

    public class Receipt : BaseDeletableModel<int>
    {
        public Receipt()
        {
            this.Products = new HashSet<ProductReceipt>();
        }

        public decimal Discount { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal TotalPriceWithDiscount { get; set; }

        public virtual ICollection<ProductReceipt> Products { get; set; }
    }
}
