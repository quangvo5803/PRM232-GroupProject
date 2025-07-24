using AutoMapper;
using BusinessObject.DTOs.ShoppingCart;
using BusinessObject.Services.Interfaces;
using DataAccess.Entities.Application;
using DataAccess.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Utilities.Exceptions;

namespace BusinessObject.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ShoppingCartService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ShoppingCartDTO>> GetCartItemsAsync(Guid userId)
        {
            var cartItems = await _unitOfWork.ShoppingCart.GetRangeAsync(
                x => x.UserId == userId,
                includeProperties: "Product,Product.ProductAvatar"
            );

            return _mapper.Map<List<ShoppingCartDTO>>(cartItems);
        }

        public async Task AddToCartAsync(ShoppingCartCreateRequestDto dto)
        {
            var existingItem = await _unitOfWork.ShoppingCart.GetAsync(x =>
                x.UserId == dto.UserId && x.ProductId == dto.ProductId
            );

            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
            }
            else
            {
                var newItem = _mapper.Map<ShoppingCart>(dto);
                newItem.Quantity = dto.Quantity;

                await _unitOfWork.ShoppingCart.AddAsync(newItem);
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateCartItemAsync(ShoppingCartUpdateRequestDto dto)
        {
            var item = await _unitOfWork.ShoppingCart.GetAsync(x => x.Id == dto.Id);

            if (item == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "Cart", new[] { "No products found in the cart." } },
                };
                throw new CustomValidationException(errors);
            }

            if (dto.Quantity == -1 && item.Quantity == 1)
            {
                _unitOfWork.ShoppingCart.Remove(item);
            }
            else
            {
                item.Quantity += dto.Quantity;
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveCartItemAsync(int cartItemId)
        {
            var item = await _unitOfWork.ShoppingCart.GetAsync(x => x.Id == cartItemId);

            if (item == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "Cart", new[] { "No products found in the cart." } },
                };
                throw new CustomValidationException(errors);
            }

            _unitOfWork.ShoppingCart.Remove(item);
            await _unitOfWork.SaveAsync();
        }
    }
}
