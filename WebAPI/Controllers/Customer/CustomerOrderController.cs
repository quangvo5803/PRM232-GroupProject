using BusinessObject.DTOs.Category;
using BusinessObject.DTOs.Orders;
using BusinessObject.FacadeService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerOrderController : ControllerBase
    {
        private readonly IFacadeService _facadeService;

        public CustomerOrderController(IFacadeService facadeService)
        {
            _facadeService = facadeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrder()
        {
            var order = await _facadeService.Order.GetAllOrderAsync();
            return Ok(order);
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderCreateRequestDto orderCreate)
        {
            if (orderCreate == null)
            {
                return BadRequest("Order data is required.");
            }
            var rsOrder = await _facadeService.Order.CreateOrderAsync(orderCreate);

            return CreatedAtAction(
                nameof(GetOrderById),
                new { id = rsOrder.Id },
                rsOrder
            );


        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _facadeService.Order.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

    }
}
