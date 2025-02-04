﻿namespace GroceryShop.Web.ViewModels.Products
{
    using AutoMapper;
    using GroceryShop.Data.Models;
    using GroceryShop.Services.Mapping;

    public class ProductViewModel : IMapFrom<Product>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Price { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, ProductViewModel>()
                .ForMember(p => p.Price, opt => opt.MapFrom(src => CurrencyFormatter.Convert(src.Price)));
        }
    }
}
