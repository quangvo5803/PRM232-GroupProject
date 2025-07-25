using BusinessObject.DTOs.Product;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Admin
{
    public partial class AdminController : ControllerBase
    {
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
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateDto productDto)
        {
            if (productDto == null)
            {
                return BadRequest("Product data is required.");
            }
            var createdProduct = await _facadeService.Product.CreateProductAsync(productDto);
            return CreatedAtAction(
                nameof(GetProductById),
                new { id = createdProduct.Id },
                createdProduct
            );
        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductUpdateDto productDto)
        {
            if (productDto == null)
            {
                return BadRequest("Product data is required.");
            }
            var updatedProduct = await _facadeService.Product.UpdateProductAsync(productDto);
            if (updatedProduct == null)
            {
                return NotFound();
            }
            return Ok(updatedProduct);
        }

        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _facadeService.Product.DeleteProductAsync(id);
            return NoContent();
        }

        [HttpGet("GetFeedbackProduct/{id}")]
        public async Task<IActionResult> GetFeedbackProductAdmin(int id)
        {
            var feedBack = await _facadeService.Feedback.GetFeedbackProductAdminAsync(id);
            return Ok(feedBack);
        }
    }
}
