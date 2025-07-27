using DataAccess.Entities.Application;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Admin
{
    public partial class AdminController : ControllerBase
    {
        [HttpGet("GetAllOrders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _facadeService.Order.GetAllOrderAsync();
            return Ok(orders);
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
    }
}
