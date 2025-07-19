using BusinessObject.DTOs.Category;

namespace BusinessObject.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDto> CreateCategoryAsync(CategoryCreateRequestDto requestDto);
        Task<CategoryDto> GetCategoryByIdAsync(int id);
        Task<IEnumerable<CategoryDto>> GetAllCategoryAsync();
        Task<CategoryDto> UpdateCategoryAsync(CategoryUpdateRequestDto requestDto);
        Task DeleteCategoryAsync(int id);
    }
}
