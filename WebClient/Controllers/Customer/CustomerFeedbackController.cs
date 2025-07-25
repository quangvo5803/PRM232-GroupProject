using Microsoft.AspNetCore.Mvc;

namespace WebClient.Controllers.Customer
{
    public partial class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
