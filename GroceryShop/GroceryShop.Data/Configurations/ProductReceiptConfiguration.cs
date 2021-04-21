namespace GroceryShop.Data.Configurations
{
    using GroceryShop.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class ProductReceiptConfiguration : IEntityTypeConfiguration<ProductReceipt>
    {
        public void Configure(EntityTypeBuilder<ProductReceipt> builder)
        {
            builder.HasKey(k => new { k.ReceiptId, k.ProductId });

            builder
                .HasOne(pr => pr.Receipt)
                .WithMany(r => r.Products)
                .HasForeignKey(pr => pr.ReceiptId);

            builder
               .HasOne(pr => pr.Product)
               .WithMany(p => p.Receipts)
               .HasForeignKey(pr => pr.ProductId);
        }
    }
}
