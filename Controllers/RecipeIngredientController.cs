using AllaCookidoo.Models;
using AllaCookidoo.Responses;
using AllaCookidoo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AllaCookidoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeIngredientController : ControllerBase
    {
        private readonly IRecipeIngredientService _recipeIngredientService;
        private readonly ILogger<RecipeIngredientController> _logger;

        public RecipeIngredientController(IRecipeIngredientService recipeIngredientService, ILogger<RecipeIngredientController> logger)
        {
            _recipeIngredientService = recipeIngredientService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipeIngredientResponse>>> GetRecipeIngredients()
        {
            _logger.LogInformation("Fetching all recipe ingredients");
            var recipeIngredients = await _recipeIngredientService.GetRecipeIngredients();
            return Ok(recipeIngredients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeIngredientResponse>> GetRecipeIngredientById(int id)
        {
            _logger.LogInformation("Fetching recipe ingredient with ID: {RecipeIngredientId}", id);
            var recipeIngredient = await _recipeIngredientService.GetRecipeIngredientById(id);
            if (recipeIngredient == null)
            {
                _logger.LogWarning("Recipe ingredient with ID {RecipeIngredientId} not found", id);
                return NotFound();
            }
            return Ok(recipeIngredient);
        }

        [HttpPost]
        public async Task<ActionResult> PostRecipeIngredient(RecipeIngredientRequest recipeIngredientCreation)
        {
            _logger.LogInformation("Adding new recipe ingredient");
            await _recipeIngredientService.AddRecipeIngredient(recipeIngredientCreation);
            return CreatedAtAction(nameof(GetRecipeIngredientById), new { id = recipeIngredientCreation.RecipeIngredientId }, recipeIngredientCreation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipeIngredient(int id, RecipeIngredientResponse recipeIngredientUpdate)
        {
            _logger.LogInformation("Updating recipe ingredient with ID: {RecipeIngredientId}", id);
            if (id != recipeIngredientUpdate.RecipeIngredientId)
            {
                _logger.LogWarning("Recipe ingredient ID in URL ({UrlId}) does not match with recipe ingredient ID in content ({ContentId})", id, recipeIngredientUpdate.RecipeIngredientId);
                return BadRequest();
            }

            await _recipeIngredientService.UpdateRecipeIngredient(id, recipeIngredientUpdate);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipeIngredient(int id)
        {
            _logger.LogInformation("Deleting recipe ingredient with ID: {RecipeIngredientId}", id);
            await _recipeIngredientService.DeleteRecipeIngredient(id);
            return NoContent();
        }
    }
}
