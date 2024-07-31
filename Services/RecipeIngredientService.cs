using AllaCookidoo.Entities;
using AllaCookidoo.Models;
using AllaCookidoo.Repositories;
using AllaCookidoo.Responses;

namespace AllaCookidoo.Services
{
    public class RecipeIngredientService : IRecipeIngredientService
    {
        private readonly IRecipeIngredientRepository _recipeIngredientRepository;
        private readonly ILogger<RecipeIngredientService> _logger;
        private readonly IRecipeRepository _recipeRepository;
        private IIngredientRepository _ingredientRepository;


        public RecipeIngredientService(IRecipeIngredientRepository recipeIngredientRepository, ILogger<RecipeIngredientService> logger,
            IRecipeRepository recipeRepository, IIngredientRepository ingredientRepository)
        {
            _recipeIngredientRepository = recipeIngredientRepository;
            _logger = logger;
            _recipeRepository = recipeRepository;
            _ingredientRepository = ingredientRepository;
        }

        public async Task<IEnumerable<RecipeIngredientResponse>> GetRecipeIngredients()
        {
            _logger.LogInformation("Fetching all recipe ingredients");
            var recipeIngredients = await _recipeIngredientRepository.GetRecipeIngredients();
            return recipeIngredients.Select(recipeIngredient => new RecipeIngredientResponse
            {
                RecipeIngredientId = recipeIngredient.RecipeIngredientId,
                IngredientId = recipeIngredient.IngredientId,
                Amount = recipeIngredient.Amount,
            }).ToList();
        }

        public async Task<RecipeIngredientResponse> GetRecipeIngredientById(int id)
        {
            _logger.LogInformation("Fetching recipe ingredient with ID: {RecipeIngredientId}", id);
            var recipeIngredient = await _recipeIngredientRepository.GetRecipeIngredientById(id);
            if (recipeIngredient == null)
            {
                _logger.LogWarning("Recipe ingredient with ID {RecipeIngredientId} not found", id);
                return null;
            }

            return new RecipeIngredientResponse
            {
                RecipeIngredientId = recipeIngredient.RecipeIngredientId,
                IngredientId = recipeIngredient.IngredientId,
                Amount = recipeIngredient.Amount,
            };
        }

        public async Task AddRecipeIngredient(RecipeIngredientRequest recipeIngredientCreation)
        {
            var recipeExists = await _recipeRepository.GetRecipeById(recipeIngredientCreation.RecipeId);
            if (recipeExists == null)
            {
                _logger.LogWarning("Recipe with ID {Id} does not exist", recipeIngredientCreation.RecipeId);
                throw new ArgumentException($"Recipe with ID {recipeIngredientCreation.RecipeId} does not exist.");
            }
            var ingredientExists = await _ingredientRepository.GetIngredientById(recipeIngredientCreation.IngredientId);
            if (ingredientExists == null)
            {
                _logger.LogWarning("Ingredient with ID {Id} does not exist", recipeIngredientCreation.IngredientId);
                throw new ArgumentException($"Ingredient with ID {recipeIngredientCreation.IngredientId} does not exist.");
            }
            _logger.LogInformation("Adding new recipe ingredient");
            var recipeIngredientEntity = new RecipeIngredientEntity
            {
                RecipeId = recipeIngredientCreation.RecipeId,
                IngredientId = recipeIngredientCreation.IngredientId,
                Amount = recipeIngredientCreation.Amount,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
            };

            await _recipeIngredientRepository.AddRecipeIngredient(recipeIngredientEntity);
        }

        public async Task UpdateRecipeIngredient(int id, RecipeIngredientResponse recipeIngredientUpdate)
        {
            var ingredientExists = await _ingredientRepository.GetIngredientById(recipeIngredientUpdate.IngredientId);
            if (ingredientExists == null)
            {
                _logger.LogWarning("Ingredient with ID {Id} does not exist", recipeIngredientUpdate.IngredientId);
                throw new ArgumentException($"Ingredient with ID {recipeIngredientUpdate.IngredientId} does not exist.");
            }
            _logger.LogInformation("Updating recipe ingredient with ID: {RecipeIngredientId}", id);
            var recipeIngredientEntity = await _recipeIngredientRepository.GetRecipeIngredientById(id);
            if (recipeIngredientEntity == null)
            {
                _logger.LogWarning("Recipe ingredient with ID {RecipeIngredientId} not found", id);
                throw new KeyNotFoundException("Recipe ingredient not found");
            }

            recipeIngredientEntity.IngredientId = recipeIngredientUpdate.IngredientId;
            recipeIngredientEntity.Amount = recipeIngredientUpdate.Amount;
            recipeIngredientEntity.UpdatedDate = DateTime.UtcNow;

            await _recipeIngredientRepository.UpdateRecipeIngredient(recipeIngredientEntity);
        }

        public async Task DeleteRecipeIngredient(int id)
        {
            _logger.LogInformation("Deleting recipe ingredient with ID: {RecipeIngredientId}", id);
            try
            {
                var RecipeIngredientEntity = await _recipeIngredientRepository.GetRecipeIngredientById(id);
                if (RecipeIngredientEntity == null)
                {
                    _logger.LogWarning("Recipe ingredient with ID {Id} not found", id);
                    throw new KeyNotFoundException("RecipeIngredient not found");
                }

                RecipeIngredientEntity.IsDeleted = true;
                RecipeIngredientEntity.UpdatedDate = DateTime.UtcNow;

                await _recipeIngredientRepository.UpdateRecipeIngredient(RecipeIngredientEntity);
                _logger.LogDebug("Deleted recipe ingredient with ID {Id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting recipe ingredient with ID {Id}", id);
                throw;
            }
        }

    }
}
