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
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductCreateDto, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ProductAvatarId, opt => opt.Ignore())
                .ForMember(dest => dest.ProductAvatar, opt => opt.Ignore())
                .ForMember(dest => dest.ProductImages, opt => opt.Ignore())
                .ForMember(dest => dest.Feedbacks, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));
            CreateMap<ProductUpdateDto, Product>()
                .ForMember(dest => dest.ProductAvatarId, opt => opt.Ignore())
                .ForMember(dest => dest.ProductAvatar, opt => opt.Ignore())
                .ForMember(dest => dest.ProductImages, opt => opt.Ignore())
                .ForMember(dest => dest.Feedbacks, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore());
        }
    }
}
