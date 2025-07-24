using BusinessObject.DTOs.ShoppingCart;

namespace BusinessObject.Services.Interfaces
{
    public interface IShoppingCartService
    {
        Task<List<ShoppingCartDTO>> GetCartItemsAsync(Guid userId);
        Task AddToCartAsync(ShoppingCartCreateRequestDto cartDto);
        Task UpdateCartItemAsync(ShoppingCartUpdateRequestDto cartDto);
        Task RemoveCartItemAsync(int cartItemId);
    }

}
