﻿using AutoMapper;
using BusinessObject.DTOs.Category;
using BusinessObject.DTOs.OrderDetail;
using BusinessObject.DTOs.Orders;
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

            //Order
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails));
            CreateMap<OrderCreateRequestDto, Order>();

            //OrderDetail
            CreateMap<OrderDetail, OrderDetailDto>();
            CreateMap<OrderDetailCreateRequestDto, OrderDetail>();

        }
    }
}
