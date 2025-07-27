using System.Text;
using AutoMapper;
using BusinessObject.DTOs.Orders;
using BusinessObject.Services.Interfaces;
using DataAccess.Entities.Application;
using DataAccess.Entities.Authorize;
using DataAccess.UnitOfWork;
using MailKit.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Utilities.Exceptions;

namespace BusinessObject.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IVnPayService _vpnPayService;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IVnPayService vnPayService,
            UserManager<ApplicationUser> userManager
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _vpnPayService = vnPayService;
            _userManager = userManager;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrderAsync()
        {
            var order = await _unitOfWork.Order.GetAllAsync(includeProperties: "OrderDetails");

            var rsMapper = _mapper.Map<IEnumerable<OrderDto>>(order ?? new List<Order>());
            foreach (var orderDto in rsMapper)
            {
                var user = await _userManager.FindByIdAsync(orderDto.UserId.ToString());
                if (user != null)
                {
                    orderDto.UserFullName = user.FullName;
                }
            }
            return rsMapper;
        }

        public async Task<OrderDto> CreateOrderAsync(OrderCreateRequestDto requestDto)
        {
            var rsMapper = _mapper.Map<Order>(requestDto);
            rsMapper.TotalPrice = 0;
            rsMapper.OrderDetails = new List<OrderDetail>();

            foreach (var detailDto in requestDto.OrderDetails)
            {
                var product = await _unitOfWork.Product.GetAsync(p => p.Id == detailDto.ProductId);
                if (product == null)
                {
                    var errors = new Dictionary<string, string[]>
                    {
                        { "Product", new[] { "Product not found." } },
                    };
                    throw new CustomValidationException(errors);
                }

                var orderDetail = new OrderDetail
                {
                    ProductId = detailDto.ProductId,
                    Quantity = detailDto.Quantity,
                    UnitPrice = product.Price,
                };

                rsMapper.TotalPrice += orderDetail.Quantity * orderDetail.UnitPrice;
                rsMapper.OrderDetails.Add(orderDetail);
            }

            await _unitOfWork.Order.AddAsync(rsMapper);
            await _unitOfWork.SaveAsync();
            var cartItems = await _unitOfWork.ShoppingCart.GetRangeAsync(c =>
                c.UserId == rsMapper.UserId
            );
            if (cartItems != null)
            {
                _unitOfWork.ShoppingCart.RemoveRange(cartItems);
                await _unitOfWork.SaveAsync();
            }
            return _mapper.Map<OrderDto>(rsMapper);
        }

        public async Task<string> CreateVNPayPaymentUrlAsync(
            OrderCreateRequestDto requestDto,
            HttpContext context
        )
        {
            var rsMapper = _mapper.Map<Order>(requestDto);
            rsMapper.TotalPrice = 0;
            rsMapper.OrderDetails = new List<OrderDetail>();

            foreach (var detailDto in requestDto.OrderDetails)
            {
                var product = await _unitOfWork.Product.GetAsync(p => p.Id == detailDto.ProductId);
                if (product == null)
                {
                    var errors = new Dictionary<string, string[]>
                    {
                        { "Product", new[] { "Product not found." } },
                    };
                    throw new CustomValidationException(errors);
                }

                var orderDetail = new OrderDetail
                {
                    ProductId = detailDto.ProductId,
                    Quantity = detailDto.Quantity,
                    UnitPrice = product.Price,
                };

                rsMapper.TotalPrice += orderDetail.Quantity * orderDetail.UnitPrice;
                rsMapper.OrderDetails.Add(orderDetail);
            }

            await _unitOfWork.Order.AddAsync(rsMapper);
            await _unitOfWork.SaveAsync();
            var cartItems = await _unitOfWork.ShoppingCart.GetRangeAsync(c =>
                c.UserId == rsMapper.UserId
            );
            if (cartItems != null)
            {
                _unitOfWork.ShoppingCart.RemoveRange(cartItems);
                await _unitOfWork.SaveAsync();
            }
            var vnPayRequest = new VnPaymentRequestModel
            {
                OrderId = rsMapper.Id,
                Amount = rsMapper.TotalPrice,
                CreateDate = DateTime.Now,
                Description = "Thanh toán VNPay",
            };

            var url = _vpnPayService.CreatePaymentUrl(context, vnPayRequest, "Buy");
            return url;
        }

        public async Task<bool> VNPayCallbackAsync(IQueryCollection query)
        {
            var vnResponse = _vpnPayService.PaymentExecute(query);

            if (vnResponse == null || vnResponse.VnPayResponseCode != "00")
            {
                if (int.TryParse(vnResponse?.OrderId, out var failOrderId))
                {
                    var order = await _unitOfWork.Order.GetAsync(o => o.Id == failOrderId);
                    if (order != null)
                    {
                        _unitOfWork.Order.Remove(order);
                        await _unitOfWork.SaveAsync();
                    }
                }

                return false;
            }
            return true;
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _unitOfWork.Order.GetAsync(
                o => o.Id == id,
                includeProperties: "OrderDetails"
            );
            if (order == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "Order", new[] { "Order not found." } },
                };
                throw new CustomValidationException(errors);
            }

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<List<OrderDto>> GetOrdersByUserIdAsync(Guid userId)
        {
            var orders = await _unitOfWork.Order.GetRangeAsync(
                o => o.UserId == userId,
                includeProperties: "OrderDetails,OrderDetails.Product,OrderDetails.Product.Category,OrderDetails.Product.ProductAvatar"
            );

            return _mapper.Map<List<OrderDto>>(orders);
        }

        public async Task<List<CheckOutDto>> CheckOutAsync(Guid userId)
        {
            var checkOut = await _unitOfWork.ShoppingCart.GetRangeAsync(
                c => c.UserId == userId,
                includeProperties: "Product,Product.ProductAvatar"
            );
            var rsMapper = _mapper.Map<List<CheckOutDto>>(checkOut);
            return rsMapper;
        }

        public async Task CancelOrderAsync(int id)
        {
            var order = await _unitOfWork.Order.GetAsync(
                o => o.Id == id,
                includeProperties: "OrderDetails"
            );
            if (order == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "order", new[] { "Order not found." } },
                };
                throw new CustomValidationException(errors);
            }
            order.Status = OrderStatus.Cancelled;
            await _unitOfWork.SaveAsync();
        }
    }
}
