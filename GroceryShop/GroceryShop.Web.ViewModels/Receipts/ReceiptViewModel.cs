namespace GroceryShop.Web.ViewModels.Receipts
{
    using AutoMapper;
    using GroceryShop.Data.Models;
    using GroceryShop.Services.Mapping;
    using System.Collections.Generic;
    using System.Linq;

    public class ReceiptViewModel : IMapFrom<Receipt>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string TotalPrice { get; set; }

        public string Discount { get; set; }

        public string TotalPriceWithDiscount { get; set; }

        public IEnumerable<ProductInReceiptViewModel> Products { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Receipt, ReceiptViewModel>()
                .ForMember(dest => dest.Products, options =>
                {
                    options.MapFrom(src => src.Products.Select(pr => pr.Product));
                })
                .ForMember(r => r.TotalPrice, opt => opt.MapFrom(src => CurrencyFormatter.Convert(src.TotalPrice)))
                .ForMember(r => r.Discount, opt => opt.MapFrom(src => CurrencyFormatter.Convert(src.Discount)))
                .ForMember(r => r.TotalPriceWithDiscount, opt => opt.MapFrom(src => CurrencyFormatter.Convert(src.TotalPriceWithDiscount)));
        }
    }
}
