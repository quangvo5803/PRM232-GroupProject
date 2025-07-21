using AutoMapper;
using BusinessObject.DTOs.Category;
using BusinessObject.Services.Interfaces;
using DataAccess.Entities.Application;
using DataAccess.UnitOfWork;
using Utilities.Exceptions;
using Utilities.Extensions;

namespace BusinessObject.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoryCreateRequestDto requestDto)
        {
            var category = _mapper.Map<Category>(requestDto);
            await _unitOfWork.Category.AddAsync(category);
            await _unitOfWork.SaveAsync();

            var categoryDto = _mapper.Map<CategoryDto>(category);
            return categoryDto;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Category.GetAsync(c => c.Id == id);
            if (category == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "Category", new[] { "Category not found." } },
                };
                throw new CustomValidationException(errors);
            }
            _unitOfWork.Category.Remove(category);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoryAsync()
        {
            var categories = await _unitOfWork.Category.GetAllAsync();

            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(
                categories ?? new List<Category>()
            );
            return categoryDtos;
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.Category.GetAsync(c => c.Id == id);
            if (category == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "Category", new[] { "Category not found." } },
                };
                throw new CustomValidationException(errors);
            }
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return categoryDto;
        }

        public async Task<CategoryDto> UpdateCategoryAsync(CategoryUpdateRequestDto requestDto)
        {
            var category = await _unitOfWork.Category.GetAsync(c => c.Id == requestDto.Id);
            if (category == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "Category", new[] { "Category not found." } },
                };
                throw new CustomValidationException(errors);
            }
            category.PatchFrom(requestDto);
            await _unitOfWork.SaveAsync();

            var categoryDto = _mapper.Map<CategoryDto>(category);
            return categoryDto;
        }
    }
}
