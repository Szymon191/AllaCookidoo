using AllaCookidoo.Models;
using AllaCookidoo.Responses;
using AllaCookidoo.Services;
using Microsoft.AspNetCore.Mvc;

namespace AllaCookidoo.Controls
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategories()
        {
            _logger.LogInformation("Fetching all categories");
            var categories = await _categoryService.GetCategories();

            if (categories == null)
            {
                return NotFound();
            }

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryResponse>> GetCategoryById(int id)
        {
            _logger.LogInformation("Fetching category with ID: {CategoryId}", id);
            var category = await _categoryService.GetCategoryById(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult> PostCategory(CategoryRequest categoryCreation)
        {
            _logger.LogInformation("Adding new category: {CategoryName}", categoryCreation.Name);
            await _categoryService.AddCategory(categoryCreation);
            return CreatedAtAction(nameof(GetCategoryById), new { id = categoryCreation.CategoryId }, categoryCreation);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            _logger.LogInformation("Deleting category with ID: {CategoryId}", id);
            await _categoryService.DeleteCategory(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryResponse categoryUpdate)
        {
            _logger.LogInformation("Updating category with ID: {CategoryId}", id);
            if (id != categoryUpdate.CategoryId)
            {
                return BadRequest();
            }

            await _categoryService.UpdateCategory(id, categoryUpdate);
            return NoContent();
        }
    }
}
