using AutoMapper;
using BusinessObject.DTOs.ShoppingCart;
using BusinessObject.Services.Interfaces;
using DataAccess.Entities.Application;
using DataAccess.UnitOfWork;
using Microsoft.EntityFrameworkCore;

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
            var cartItems = await _unitOfWork.ShoppingCart
          .GetAsync(x => x.UserId == userId);

            return _mapper.Map<List<ShoppingCartDTO>>(cartItems);
        }

        public async Task AddToCartAsync(Guid userId, int productId, int quantity)
        {
            var existingItem = await _unitOfWork.ShoppingCart.GetAsync(
                x => x.UserId == userId && x.ProductId == productId
            );

            if (existingItem != null)
            {
                existingItem.Count += quantity;
            }
            else
            {
                var newItem = new ShoppingCart
                {
                    UserId = userId,
                    ProductId = productId,
                    Count = quantity
                };

                await _unitOfWork.ShoppingCart.AddAsync(newItem);
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateCartItemAsync(int cartItemId, int quantity)
        {
            var item = await _unitOfWork.ShoppingCart.GetAsync(x => x.Id == cartItemId);

            if (item == null)
                throw new Exception("Không tìm thấy sản phẩm trong giỏ hàng.");

            if (quantity == -1 && item.Count == 1)
                _unitOfWork.ShoppingCart.Remove(item);
            else
                item.Count += quantity;

            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveCartItemAsync(int cartItemId)
        {
            var item = await _unitOfWork.ShoppingCart.GetAsync(x => x.Id == cartItemId);

            if (item == null)
                throw new Exception("Không tìm thấy sản phẩm trong giỏ hàng.");

            _unitOfWork.ShoppingCart.Remove(item);
            await _unitOfWork.SaveAsync();
        }
    }
}
