namespace GroceryShop.Data.Models
{
    using GroceryShop.Data.Common.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Product : BaseDeletableModel<int>
    {
        public Product()
        {
            this.Deals = new HashSet<ProductDeal>();
            this.Receipts = new HashSet<ProductReceipt>();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        public virtual ICollection<ProductDeal> Deals { get; set; }

        public virtual ICollection<ProductReceipt> Receipts { get; set; }
    }
}
