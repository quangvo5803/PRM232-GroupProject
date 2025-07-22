using BusinessObject.DTOs.ShoppingCart;

namespace BusinessObject.Services.Interfaces
{
    public interface IShoppingCartService
    {
        Task<List<ShoppingCartDTO>> GetCartItemsAsync(Guid userId);
        Task AddToCartAsync(Guid userId, int productId, int quantity);
        Task UpdateCartItemAsync(int cartItemId, int quantity);
        Task RemoveCartItemAsync(int cartItemId);
    }
}
