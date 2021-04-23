namespace GroceryShop.Web.ViewModels.Receipts
{
    using AutoMapper;
    using GroceryShop.Data.Models;
    using GroceryShop.Services.Mapping;

    public class ProductInReceiptViewModel : IMapFrom<Product>, IHaveCustomMappings
    {
        public string Name { get; set; }

        public string Price { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, ProductInReceiptViewModel>()
                .ForMember(p => p.Price, opt => opt.MapFrom(src => CurrencyFormatter.Convert(src.Price)));
        }
    }
}
