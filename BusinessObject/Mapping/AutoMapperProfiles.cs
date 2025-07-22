using AutoMapper;
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
