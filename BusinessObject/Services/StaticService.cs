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
            var allOrders = await _oderService.GetAllOrderAsync();

            var orderCompleted = allOrders.Where(o => o.Status == OrderStatus.Completed.ToString()).ToList();
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            // Total orders in current month
            var monthlyOrders = orderCompleted.Where(o => o.OrderDate.Month == currentMonth && o.OrderDate.Year == currentYear).ToList();
            var totalOrdersMonth = monthlyOrders;
            var earningsMonth = monthlyOrders.Sum(o => o.TotalPrice);

            // Total prices per month
            var totalPricesEachMonth = Enumerable.Range(1, 12)
                .Select(m =>
                    orderCompleted
                        .Where(o => o.OrderDate.Month == m && o.OrderDate.Year == currentYear)
                        .Sum(o => o.TotalPrice)
                )
                .ToList();

            // User count
            var customers = await _userService.GetAllCustomersAsync();
            var userCount = customers.Count();

            // Average Rating
            var products = await _productService.GetAllProductAsync();
            var ratings = new List<int>();
            foreach (var product in products)
            {
                var feedback = await _feedbackService.GetFeedbackProductAdminAsync(product.Id);
                if (feedback.Feedbacks != null && feedback.Feedbacks.Any())
                {
                    ratings.AddRange(feedback.Feedbacks.Select(f => f.FeedbackStars));
                }
            }
            var avgRating = ratings.Count > 0 ? ratings.Average() : 0;

            // Categories and quantities sold
            var categories = await _categoryService.GetAllCategoryAsync();
            var categoryNames = new List<string>();
            var totalQuantities = new List<int>();

            foreach (var category in categories)
            {
                var productIdsInCategory = products
                    .Where(p => p.Id == category.Id)
                    .Select(p => p.Id)
                    .ToList();

                var quantitySold = orderCompleted
                    .SelectMany(o => o.OrderDetails)
                    .Where(od => productIdsInCategory.Contains(od.Id))
                    .Sum(od => od.Quantity);

                categoryNames.Add(category.Name);
                totalQuantities.Add(quantitySold);
            }

            return new StaticDto
            {
                TotalOrdersMonth = totalOrdersMonth.Count,
                EarningsMonth = earningsMonth,
                UserCount = userCount,
                AvgRating = avgRating,
                TotalPricesEachMonth = totalPricesEachMonth,
                CategoriesName = categoryNames,
                TotalQuantity = totalQuantities
            };
        }

    }
}
