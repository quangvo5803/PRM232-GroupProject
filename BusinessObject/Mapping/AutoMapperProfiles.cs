using AutoMapper;
using BusinessObject.DTOs.Category;
using BusinessObject.DTOs.OrderDetail;
using BusinessObject.DTOs.Orders;
using BusinessObject.DTOs.Product;
using BusinessObject.DTOs.ShoppingCart;
using BusinessObject.DTOs.User;
using DataAccess.Entities.Application;
using DataAccess.Entities.Authorize;
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

            CreateMap<ApplicationUser, UserDto>();
            CreateMap<UserUpdateRequestDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore());

            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CategoryCreateRequestDto>().ReverseMap();
            CreateMap<Category, CategoryUpdateRequestDto>().ReverseMap();

            //Order
            CreateMap<Order, OrderDto>()
                .ForMember(
                    dest => dest.PaymentMethod,
                    opt => opt.MapFrom(src => src.PaymentMethod.ToString())
                )
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails));
            CreateMap<OrderCreateRequestDto, Order>();

            //OrderDetail
            CreateMap<OrderDetail, OrderDetailDto>();
            CreateMap<OrderDetailCreateRequestDto, OrderDetail>();

            //CheckOut
            CreateMap<ShoppingCart, CheckOutDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product!.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product!.Price))
                .ForMember(
                    dest => dest.ProductAvatar,
                    opt =>
                        opt.MapFrom(src =>
                            src.Product!.ProductAvatar != null
                                ? src.Product.ProductAvatar.ImageUrl
                                : null
                        )
                )
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Quantity));

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
