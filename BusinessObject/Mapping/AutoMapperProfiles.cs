using AutoMapper;
using BusinessObject.DTOs.ShoppingCart;
using DataAccess.Entities.Application;
using Utilities.Extensions;

namespace BusinessObject.Mapping
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<OrderStatus, string>().ConvertUsing(src => src.GetDisplayName());

            // Map ShopppingCart to ShopppingCartDTO
            CreateMap<ShoppingCart, ShoppingCartDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price));

            CreateMap<ShoppingCartDTO, ShoppingCart>();
        }
    }
}
