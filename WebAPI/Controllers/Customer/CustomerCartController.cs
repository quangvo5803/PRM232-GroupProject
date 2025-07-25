﻿using BusinessObject.DTOs.ShoppingCart;
using BusinessObject.FacadeService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Customer
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
    [Route("api/[controller]")]
    [ApiController]
    public partial class CustomerController : ControllerBase
    {
        private readonly IFacadeService _facadeService;

        public CustomerController(IFacadeService facadeService)
        {
            _facadeService = facadeService;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<IActionResult> GetCartItems(Guid userId)
        {
            var items = await _facadeService.ShoppingCart.GetCartItemsAsync(userId);
            return Ok(items);
        }

        [HttpPost("AddItem")]
        public async Task<IActionResult> AddToCart([FromBody] ShoppingCartCreateRequestDto cartDto)
        {
            if (cartDto == null)
                return BadRequest("Cart data is required.");

            await _facadeService.ShoppingCart.AddToCartAsync(cartDto);
            return Ok("Sản phẩm đã được thêm vào giỏ hàng.");
        }

        [HttpPut("UpdateItem")]
        public async Task<IActionResult> UpdateCartItem(
            [FromBody] ShoppingCartUpdateRequestDto cartDto
        )
        {
            if (cartDto == null)
                return BadRequest("Cart data is required.");

            try
            {
                await _facadeService.ShoppingCart.UpdateCartItemAsync(cartDto);
                return Ok("Cập nhật giỏ hàng thành công.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("DeleteItem/{cartItemId}")]
        public async Task<IActionResult> RemoveCartItem(int cartItemId)
        {
            try
            {
                await _facadeService.ShoppingCart.RemoveCartItemAsync(cartItemId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
