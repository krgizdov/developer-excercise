namespace GroceryShop.Web.ViewModels.Deals
{
    using AutoMapper;
    using GroceryShop.Data.Models;
    using GroceryShop.Services.Mapping;
    using System.Collections.Generic;
    using System.Linq;

    public class DealViewModel : IMapFrom<Deal>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<ProductInDealViewModel> Products { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Deal, DealViewModel>()
                .ForMember(dest => dest.Products, options =>
                {
                    options.MapFrom(src => src.Products.Select(pd => pd.Product));
                });
        }
    }
}
