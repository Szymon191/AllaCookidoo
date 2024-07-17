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
            _logger.LogInformation("Pobieranie wszystkich przepisów");
            var recipes = await _recipeService.GetRecipes();

            return Ok(recipes);
        }

        [HttpGet("getRecipesfromCategorii/{id}")]
        public async Task<ActionResult<IEnumerable<RecipeResponse>>> GetRecipesFromCategory(int id)
        {
            _logger.LogInformation("Pobieranie przepisów o danej kategorii o ID: {CategoryId}", id);
            var recipes = await _recipeService.GetRecipesFromCategory(id);
            return Ok(recipes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeResponse>> GetRecipeById(int id)
        {
            _logger.LogInformation("Pobieranie przepisu o ID: {RecipeId}", id);
            var recipe = await _recipeService.GetRecipeById(id);
            if (recipe == null)
            {
                _logger.LogWarning("Przepis o ID {RecipeId} nie został znaleziony", id);
                return NotFound();
            }

            return Ok(recipe);

        }

        [HttpPost]
        public async Task<ActionResult> PostRecipe(RecipeRequest recipeCreation)
        {
            _logger.LogInformation("Dodawanie nowego przepisu");
            await _recipeService.AddRecipe(recipeCreation);
            return CreatedAtAction(nameof(GetRecipeById), new { id = recipeCreation.RecipeId }, recipeCreation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, RecipeResponse recipeUpdate)
        {
            _logger.LogInformation("Aktualizacja przepisu o ID: {RecipeId}", id);
            if (id != recipeUpdate.RecipeId)
            {
                _logger.LogWarning("ID przepisu w URL: { UrlId} nie zgadza się z ID przepisu w treści: { ContentId}", id, recipeUpdate.RecipeId);
                return BadRequest();
            }

            await _recipeService.UpdateRecipe(id, recipeUpdate);

            return Created();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            _logger.LogInformation("Usuwanie przepisu o ID: {RecipeId}", id);
            await _recipeService.DeleteRecipe(id);
            return Created();
        }
    }
}
