using AllaCookidoo.Models;
using AllaCookidoo.Responses;
using AllaCookidoo.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AllaCookidoo.Controls
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;
        private readonly ILogger<RecipeController> _logger;

        public RecipeController(IRecipeService recipeService, ILogger<RecipeController> logger)
        {
            _recipeService = recipeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipeResponse>>> GetRecipes()
        {
            _logger.LogInformation("Fetching all recipes");
            var recipes = await _recipeService.GetRecipes();

            return Ok(recipes);
        }

        [HttpGet("getRecipesfromCategorii/{id}")]
        public async Task<ActionResult<IEnumerable<RecipeResponse>>> GetRecipesFromCategory(int id)
        {
            _logger.LogInformation("Fetching recipes for category: {CategoryId}", id);
            var recipes = await _recipeService.GetRecipesFromCategory(id);
            return Ok(recipes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeResponse>> GetRecipeById(int id)
        {
            _logger.LogInformation("Fetching recipe with ID: {RecipeId}", id);
            var recipe = await _recipeService.GetRecipeById(id);
            if (recipe == null)
            {
                _logger.LogWarning("Przepis o ID {RecipeId} nie został znaleziony", id);
                return NotFound();
            }
            return Ok(recipe);
        }

        [HttpGet("GetDetailsRecipeById:{id}")]
        public async Task<ActionResult<RecipeDetailsResponse>> GetDetailsRecipeById(int id)
        {
            _logger.LogInformation("Fetching details of recipe with ID: {RecipeId}", id);
            try
            {
                var recipe = await _recipeService.GetRecipeDetailsById(id);
                if (recipe == null)
                {
                    _logger.LogWarning("Recipe with ID {RecipeId} was not found", id);
                    return NotFound();
                }
                return Ok(recipe);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching details recipe with ID {RecipeId}", id);
                return StatusCode(500, "Serwer Error.");
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostRecipe(RecipeRequest recipeCreation)
        {
            _logger.LogInformation("Adding new recipe");
            if(recipeCreation == null)
            {
                return NotFound();
            }
            await _recipeService.AddRecipe(recipeCreation);
            return CreatedAtAction(nameof(GetRecipeById), new { id = recipeCreation.RecipeId }, recipeCreation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, RecipeDetailsResponse recipeUpdate)
        {
            _logger.LogInformation("Updating recipe with ID: {RecipeId}", id);
            if (id != recipeUpdate.RecipeId)
            {
                _logger.LogWarning("Recipe ID in URL: {UrlId} does not match with recipe ID in content: {ContentId}", id, recipeUpdate.RecipeId);
                return BadRequest();
            }
            await _recipeService.UpdateRecipe(id, recipeUpdate);

            return Created();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            _logger.LogInformation("Deleting recipe with ID: {RecipeId}", id);
            await _recipeService.DeleteRecipe(id);
            return Created();
        }
    }
}
