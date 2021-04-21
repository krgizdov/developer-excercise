namespace GroceryShop.Data.Models
{
    using GroceryShop.Data.Common.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Deal : BaseDeletableModel<int>
    {
        public Deal()
        {
            this.Products = new HashSet<ProductDeal>();
        }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<ProductDeal> Products { get; set; }
    }
}
