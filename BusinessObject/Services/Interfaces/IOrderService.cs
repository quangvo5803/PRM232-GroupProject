
using BusinessObject.DTOs.Orders;

namespace BusinessObject.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(OrderCreateRequestDto requestDto);
        Task<OrderDto> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderDto>> GetAllOrderAsync();
    }
}
