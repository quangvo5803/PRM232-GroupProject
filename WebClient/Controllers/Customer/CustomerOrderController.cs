using Microsoft.AspNetCore.Mvc;

namespace WebClient.Controllers.Customer
{
    public class CustomerOrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
