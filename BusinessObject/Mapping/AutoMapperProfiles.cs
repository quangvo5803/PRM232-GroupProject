using AutoMapper;

using BusinessObject.DTOs.ShoppingCart;

using BusinessObject.DTOs.Category;
using BusinessObject.DTOs.Product;

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
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
            .ForMember(
                dest => dest.ProductAvatar,
                opt =>
                    opt.MapFrom(src =>
                        src.Product.ProductAvatar != null
                            ? src.Product.ProductAvatar.ImageUrl
                            : null
                    )
            );

            CreateMap<ShoppingCartCreateRequestDto, ShoppingCart>();
            CreateMap<ShoppingCartUpdateRequestDto, ShoppingCart>();


            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CategoryCreateRequestDto>().ReverseMap();
            CreateMap<Category, CategoryUpdateRequestDto>().ReverseMap();

            CreateMap<Product, ProductDto>()
                .ForMember(
                    dest => dest.ProductImages,
                    opt =>
                        opt.MapFrom(src =>
                            src.ProductImages != null
                                ? src.ProductImages.Select(i => i.ImageUrl).ToList()
                                : new List<string>()
                        )
                )
                .ForMember(
                    dest => dest.ProductAvatar,
                    opt =>
                        opt.MapFrom(src =>
                            src.ProductAvatar != null ? src.ProductAvatar.ImageUrl : null
                        )
                );
            CreateMap<ProductCreateDto, Product>()
                .ForMember(dest => dest.ProductAvatar, opt => opt.Ignore())
                .ForMember(dest => dest.ProductImages, opt => opt.Ignore());

            CreateMap<ProductUpdateDto, Product>()
                .ForMember(dest => dest.ProductAvatar, opt => opt.Ignore())
                .ForMember(dest => dest.ProductImages, opt => opt.Ignore());

        }
    }
}
