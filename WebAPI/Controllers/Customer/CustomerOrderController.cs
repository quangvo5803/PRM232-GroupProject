﻿using BusinessObject.DTOs.Category;
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

            if(orderCreate.PaymentMethod == "PayByCash")
            {
                var rsOrder = await _facadeService.Order.CreateOrderAsync(orderCreate);

                return CreatedAtAction(
                    nameof(GetOrderById),
                    new { id = rsOrder.Id },
                    rsOrder
                );
            }

            if (orderCreate.PaymentMethod == "VNPay")
            {
                if (orderCreate.PaymentMethod != "VNPay")
                {
                    return BadRequest("Invalid payment method. Use 'VNPay' only.");
                }
                var paymentUrl = await _facadeService.Order.CreateVNPayPaymentUrlAsync(orderCreate, HttpContext);
                return Ok(new { PaymentUrl = paymentUrl });
            }

            return BadRequest("Payment Fail");
        }

        //[HttpPost("create-vnpay")]
        //public async Task<IActionResult> CreateVNPayOrder([FromBody] OrderCreateRequestDto requestDto)
        //{
        //    if (requestDto.PaymentMethod != "VNPay")
        //    {
        //        return BadRequest("Invalid payment method. Use 'VNPay' only.");
        //    }

        //    var paymentUrl = await _facadeService.Order.CreateVNPayPaymentUrlAsync(requestDto, HttpContext);
        //    return Ok(new { PaymentUrl = paymentUrl });
        //}

        [HttpGet("vnpay-return")]
        public async Task<IActionResult> VNPayReturn()
        {
            var result = await _facadeService.Order.VNPayCallbackAsync(Request.Query);
            if (result)
            {
                return Redirect("https://www.facebook.com/"); 
            }

            return Redirect("https://www.facebook.com/DinhPhuc.Su/");
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
