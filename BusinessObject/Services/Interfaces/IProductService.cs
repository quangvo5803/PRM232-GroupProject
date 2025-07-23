using BusinessObject.DTOs.Product;

namespace BusinessObject.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> CreateProductAsync(ProductCreateDto requestDto);
        Task<ProductDto> UpdateProductAsync(ProductUpdateDto requestDto);
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetAllProductAsync();
        Task DeleteProductAsync(int id);
    }
} 