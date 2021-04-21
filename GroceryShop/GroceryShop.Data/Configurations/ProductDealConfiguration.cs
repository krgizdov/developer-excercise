namespace GroceryShop.Data.Configurations
{
    using GroceryShop.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    class ProductDealConfiguration : IEntityTypeConfiguration<ProductDeal>
    {
        public void Configure(EntityTypeBuilder<ProductDeal> builder)
        {
            builder.HasKey(k => new { k.DealId, k.ProductId });

            builder
                .HasOne(pd => pd.Deal)
                .WithMany(d => d.Products)
                .HasForeignKey(pd => pd.DealId);

            builder
               .HasOne(pd => pd.Product)
               .WithMany(p => p.Deals)
               .HasForeignKey(pd => pd.ProductId);
        }
    }
}
