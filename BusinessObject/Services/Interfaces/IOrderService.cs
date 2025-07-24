
using BusinessObject.DTOs.Orders;
using Microsoft.AspNetCore.Http;

namespace BusinessObject.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(OrderCreateRequestDto requestDto);
        Task<OrderDto> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderDto>> GetAllOrderAsync();
        Task<string> CreateVNPayPaymentUrlAsync(OrderCreateRequestDto requestDto, HttpContext context);
        Task<bool> VNPayCallbackAsync(IQueryCollection query);
        Task<List<CheckOutDto>> CheckOutAsync(Guid userId);
    }
}
