using BusinessObject.FacadeService;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IFacadeService _facadeService;

        public HomeController(IFacadeService facadeService)
        {
            _facadeService = facadeService;
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _facadeService.Product.GetAllProductAsync();
            return Ok(products);
        }

        [HttpGet("GetProductById/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _facadeService.Product.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound(new { Message = "Product not found." });
            }
            return Ok(product);
        }

        [HttpGet("GetAllFeedback/{id}")]
        public async Task<IActionResult> GetAllFeedback(int id)
        {
            var rs = await _facadeService.Feedback.GetAllFeedbackAsync(id);
            return Ok(rs);
        }
    }
}
