using BusinessObject.DTOs.Admin;
using BusinessObject.Services.Interfaces;
using DataAccess.Entities.Application;

namespace BusinessObject.Services
{
    public class StaticService : IStaticService
    {
        private readonly IOrderService _oderService;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IFeedbackService _feedbackService;
        private readonly IUserService _userService;
        public StaticService(
            IOrderService oderService,
            ICategoryService categoryService,
            IProductService productService,
            IFeedbackService feedbackService,
            IUserService userService
        )
        {
            _oderService = oderService;
            _categoryService = categoryService;
            _productService = productService;
            _feedbackService = feedbackService;
            _userService = userService;
        }

        public async Task<StaticDto> GetStatisticAsync()
        {

            try
            {
                var allOrders = await _oderService.GetAllOrderAsync();

                if (allOrders == null || !allOrders.Any())
                {
                    return new StaticDto();
                }

                var orderCompleted = allOrders
                    .Where(o => o.Status == OrderStatus.Completed.ToString())
                    .ToList();

                var currentMonth = DateTime.Now.Month;
                var currentYear = DateTime.Now.Year;

                // Thống kê tháng
                var monthlyOrders = orderCompleted
                    .Where(o => o.OrderDate.Month == currentMonth && o.OrderDate.Year == currentYear)
                    .ToList();

                var earningsMonth = monthlyOrders.Sum(o => o.TotalPrice);

                var totalPricesEachMonth = Enumerable.Range(1, 12)
                    .Select(m =>
                        orderCompleted
                            .Where(o => o.OrderDate.Month == m && o.OrderDate.Year == currentYear)
                            .Sum(o => o.TotalPrice)
                    )
                    .ToList();

                var customers = await _userService.GetAllCustomersAsync();
                var userCount = customers?.Count() ?? 0;

                var products = await _productService.GetAllProductAsync();

                var ratings = new List<int>();
                if (products != null)
                {
                    foreach (var product in products)
                    {
                        var feedback = await _feedbackService.GetFeedbackProductAdminAsync(product.Id);
                        if (feedback?.Feedbacks != null && feedback.Feedbacks.Any())
                        {
                            ratings.AddRange(feedback.Feedbacks.Select(f => f.FeedbackStars));
                        }
                    }
                }
                var avgRating = ratings.Count > 0 ? ratings.Average() : 0;

                // Thống kê số lượng theo Category
                var categories = await _categoryService.GetAllCategoryAsync();

                var categoryNames = new List<string>();
                var totalQuantities = new List<int>();

                if (categories != null && products != null)
                {
                    foreach (var category in categories)
                    {
                        var productIds = products
                            .Where(p => p.Category != null && p.Category.Id == category.Id)
                            .Select(p => p.Id)
                            .ToList();

                        int quantity = orderCompleted
                            .Where(o => o.OrderDetails != null) // Thêm null check
                            .SelectMany(o => o.OrderDetails)
                            .Where(od => od.Product != null && productIds.Contains(od.Product.Id))
                            .Sum(od => od.Quantity);

                        categoryNames.Add(category.Name);
                        totalQuantities.Add(quantity);

                    }
                }

                var result = new StaticDto
                {
                    TotalOrdersMonth = monthlyOrders.Count,
                    EarningsMonth = earningsMonth,
                    UserCount = userCount,
                    AvgRating = avgRating,
                    TotalPricesEachMonth = totalPricesEachMonth,
                    CategoriesName = categoryNames,
                    TotalQuantity = totalQuantities
                };


                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetStatisticAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }


    }
}
