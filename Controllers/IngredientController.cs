using AllaCookidoo.Controllers;
using AllaCookidoo.Database;
using AllaCookidoo.Models;
using AllaCookidoo.Responses;
using AllaCookidoo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllaCookidoo.Controls
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;
        private readonly ILogger<IngredientController> _logger;

        public IngredientController(IIngredientService ingredientService, ILogger<IngredientController> logger)
        {
            _ingredientService = ingredientService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngredientResponse>>> GetIngredients()
        {
            _logger.LogInformation("Fetching all ingredients");
            var feedbacks = await _ingredientService.GetIngredients();
            return Ok(feedbacks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IngredientResponse>> GetIngredientById(int id)
        {
            _logger.LogInformation("Fetching ingredient with ID: {Id}", id);
            var ingredient = await _ingredientService.GetIngredientById(id);
            if (ingredient == null)
            {
                _logger.LogWarning("skladnik o ID {Id} nie została znaleziona", id);
                return NotFound();
            }
            return Ok(ingredient);
        }

        [HttpPost]
        public async Task<ActionResult> PostIngredient(IngredientReguest ingredientRequest)
        {
            _logger.LogInformation("Adding new ingredient: {name}", ingredientRequest.Name);
            await _ingredientService.AddIngredient(ingredientRequest);
            return CreatedAtAction(nameof(GetIngredientById), new { id = ingredientRequest.Id }, ingredientRequest);


        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIngredient(int id, IngredientResponse ingredientUpdate)
        {
            _logger.LogInformation("Updating ingredient with ID: {Id}", id);
            if (id != ingredientUpdate.Id)
            {
                _logger.LogWarning("Ingredient ID in URL: {UrlId} does not match with ingredient ID in content: {ContentId}", id, ingredientUpdate.Id);
                return BadRequest();
            }

            await _ingredientService.UpdateIngredient(id, ingredientUpdate);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngredient(int id)
        {
            _logger.LogInformation("Deleting ingredient with ID: {Id}", id);
            await _ingredientService.DeleteIngredient(id);
            return NoContent();
        }


    }
}
