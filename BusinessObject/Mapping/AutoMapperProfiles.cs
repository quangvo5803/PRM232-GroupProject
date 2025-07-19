using AutoMapper;
using BusinessObject.DTOs.Category;
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
        }
    }
}
