using BusinessObject.DTOs.Category;
using BusinessObject.FacadeService;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class AdminController : ControllerBase
    {
        private readonly IFacadeService _facadeService;

        public AdminController(IFacadeService facadeService)
        {
            _facadeService = facadeService;
        }

        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _facadeService.Category.GetAllCategoryAsync();
            return Ok(categories);
        }

        [HttpGet("GetCategoryById/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _facadeService.Category.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory(
            [FromBody] CategoryCreateRequestDto categoryDto
        )
        {
            if (categoryDto == null)
            {
                return BadRequest("Category data is required.");
            }
            var createdCategory = await _facadeService.Category.CreateCategoryAsync(categoryDto);
            return CreatedAtAction(
                nameof(GetCategoryById),
                new { id = createdCategory.Id },
                createdCategory
            );
        }

        [HttpPut("UpdateCategory/{id}")]
        public async Task<IActionResult> UpdateCategory(
            int id,
            [FromBody] CategoryUpdateRequestDto categoryDto
        )
        {
            if (categoryDto == null)
            {
                return BadRequest("Category data is required.");
            }
            var updatedCategory = await _facadeService.Category.UpdateCategoryAsync(categoryDto);
            if (updatedCategory == null)
            {
                return NotFound();
            }
            return Ok(updatedCategory);
        }

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _facadeService.Category.DeleteCategoryAsync(id);

            return NoContent();
        }
    }
}
