using BusinessObject.DTOs.Category;
using BusinessObject.DTOs.Orders;
using BusinessObject.FacadeService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Customer
{
    public partial class CustomerController : ControllerBase
    {
        [HttpGet("GetAllOrder")]
        public async Task<IActionResult> GetAllOrder()
        {
            var order = await _facadeService.Order.GetAllOrderAsync();
            return Ok(order);
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder(OrderCreateRequestDto orderCreate)
        {
            if (orderCreate == null)
            {
                return BadRequest("Order data is required.");
            }

            if (orderCreate.PaymentMethod == "PayByCash")
            {
                var rsOrder = await _facadeService.Order.CreateOrderAsync(orderCreate);

                return CreatedAtAction(nameof(GetOrderById), new { id = rsOrder.Id }, rsOrder);
            }

            if (orderCreate.PaymentMethod == "VNPay")
            {
                var paymentUrl = await _facadeService.Order.CreateVNPayPaymentUrlAsync(
                    orderCreate,
                    HttpContext
                );
                return Ok(new { PaymentUrl = paymentUrl });
            }

            return BadRequest("Payment Fail");
        }

        [HttpGet("VnPayReturn")]
        [AllowAnonymous]
        public async Task<IActionResult> VNPayReturn()
        {
            var result = await _facadeService.Order.VNPayCallbackAsync(Request.Query);
            return Redirect("https://localhost:7295/");
        }

        [HttpGet("GetOrderById/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _facadeService.Order.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpGet("OrderHistory/{userId}")]
        public async Task<IActionResult> GetOrderHistory(Guid userId)
        {
            var orders = await _facadeService.Order.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }


        [HttpGet("Checkout/{userId}")]
        public async Task<IActionResult> CheckOut(Guid userId)
        {
            var request = await _facadeService.Order.CheckOutAsync(userId);
            if (request == null)
                return NotFound();
            return Ok(request);
        }
    }
}
