using AutoMapper;
using BusinessObject.DTOs.Product;
using BusinessObject.Services.Interfaces;
using DataAccess.Entities.Application;
using DataAccess.UnitOfWork;
using Utilities.Exceptions;
using Utilities.Extensions;

namespace BusinessObject.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductDto> CreateProductAsync(ProductCreateDto requestDto)
        {
            var product = _mapper.Map<Product>(requestDto);
            await _unitOfWork.Product.AddAsync(product); // Đảm bảo có Id trước khi upload ảnh
            await _unitOfWork.SaveAsync();
            if (requestDto.ProductAvatar != null)
            {
                var image = new ItemImage { ProductId = product.Id, ImageUrl = string.Empty };
                await _unitOfWork.ItemImage.UploadImageAsync(
                    requestDto.ProductAvatar,
                    "FoodHub/Product",
                    image
                );
                product.ProductAvatarId = image.ImageID;
                await _unitOfWork.SaveAsync();
            }

            if (requestDto.ProductImages != null && requestDto.ProductImages.Any())
            {
                foreach (var file in requestDto.ProductImages)
                {
                    var image = new ItemImage { ProductId = product.Id, ImageUrl = string.Empty };
                    await _unitOfWork.ItemImage.UploadImageAsync(file, "FoodHub/Product", image);
                }
            }
            var productDto = _mapper.Map<ProductDto>(product);
            return productDto;
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Product.GetAsync(p => p.Id == id);
            if (product == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "Product", new[] { "Product not found." } },
                };
                throw new CustomValidationException(errors);
            }

            // Xóa toàn bộ ảnh liên quan (trên Cloudinary và DB)
            var images = await _unitOfWork.ItemImage.GetRangeAsync(i => i.ProductId == id);
            foreach (var image in images)
            {
                await _unitOfWork.ItemImage.DeleteImageAsync(image.PublicId);
            }

            // Xóa toàn bộ feedback liên quan
            var feedbacks = await _unitOfWork.Feedback.GetRangeAsync(f => f.ProductId == id);
            _unitOfWork.Feedback.RemoveRange(feedbacks);

            // Xóa toàn bộ cart liên quan
            var carts = await _unitOfWork.ShoppingCart.GetRangeAsync(c => c.ProductId == id);
            _unitOfWork.ShoppingCart.RemoveRange(carts);

            // Xóa Product
            _unitOfWork.Product.Remove(product);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductAsync()
        {
            var products = await _unitOfWork.Product.GetAllAsync(
                includeProperties: "Category,ProductAvatar,ProductImages,Feedbacks"
            );
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products ?? new List<Product>());
            return productDtos;
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Product.GetAsync(
                p => p.Id == id,
                includeProperties: "Category,ProductAvatar,ProductImages,Feedbacks"
            );
            if (product == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "Product", new[] { "Product not found." } },
                };
                throw new CustomValidationException(errors);
            }
            var productDto = _mapper.Map<ProductDto>(product);
            return productDto;
        }

        public async Task<ProductDto> UpdateProductAsync(ProductUpdateDto requestDto)
        {
            var product = await _unitOfWork.Product.GetAsync(p => p.Id == requestDto.Id);
            if (product == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "Product", new[] { "Product not found." } },
                };
                throw new CustomValidationException(errors);
            }

            // Update các trường cơ bản
            product.PatchFrom(requestDto);

            // Nếu có upload ảnh mới
            if (requestDto.ProductAvatar != null)
            {
                // Xoá ảnh cũ nếu có
                if (product.ProductAvatarId.HasValue)
                {
                    var oldImage = await _unitOfWork.ItemImage.GetAsync(i =>
                        i.ImageID == product.ProductAvatarId
                    );
                    if (oldImage != null)
                    {
                        await _unitOfWork.ItemImage.DeleteImageAsync(oldImage.PublicId);
                    }
                }
                // Upload ảnh mới
                var image = new ItemImage { ProductId = product.Id, ImageUrl = string.Empty };
                await _unitOfWork.ItemImage.UploadImageAsync(
                    requestDto.ProductAvatar,
                    "FoodHub/Product",
                    image
                );
                product.ProductAvatarId = image.ImageID;
            }

            if (requestDto.ProductImages != null && requestDto.ProductImages.Any())
            {
                // Xoá toàn bộ ảnh cũ
                var oldImages = await _unitOfWork.ItemImage.GetRangeAsync(i =>
                    i.ProductId == product.Id
                );
                foreach (var oldImage in oldImages)
                {
                    await _unitOfWork.ItemImage.DeleteImageAsync(oldImage.PublicId);
                }
                // Upload ảnh mới
                foreach (var file in requestDto.ProductImages)
                {
                    var image = new ItemImage { ProductId = product.Id, ImageUrl = string.Empty };
                    await _unitOfWork.ItemImage.UploadImageAsync(file, "FoodHub/Product", image);
                }
            }
            await _unitOfWork.SaveAsync();
            // Lấy lại product mới nhất từ DB để trả về đúng thông tin
            var updatedProduct = await _unitOfWork.Product.GetAsync(p => p.Id == product.Id);
            var productDto = _mapper.Map<ProductDto>(updatedProduct);
            return productDto;
        }
    }
}
