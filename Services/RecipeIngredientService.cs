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

        public RecipeIngredientService(IRecipeIngredientRepository recipeIngredientRepository, ILogger<RecipeIngredientService> logger)
        {
            _recipeIngredientRepository = recipeIngredientRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<RecipeIngredientResponse>> GetRecipeIngredients()
        {
            _logger.LogInformation("Pobieranie wszystkich składników przepisu");
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
            _logger.LogInformation("Pobieranie składnika przepisu o ID: {RecipeIngredientId}", id);
            var recipeIngredient = await _recipeIngredientRepository.GetRecipeIngredientById(id);
            if (recipeIngredient == null)
            {
                _logger.LogWarning("Składnik przepisu o ID {RecipeIngredientId} nie został znaleziony", id);
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
            _logger.LogInformation("Dodawanie nowego składnika przepisu");
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
            _logger.LogInformation("Aktualizacja składnika przepisu o ID: {RecipeIngredientId}", id);
            var recipeIngredientEntity = await _recipeIngredientRepository.GetRecipeIngredientById(id);
            if (recipeIngredientEntity == null)
            {
                _logger.LogWarning("Składnik przepisu o ID {RecipeIngredientId} nie został znaleziony", id);
                throw new KeyNotFoundException("Recipe ingredient not found");
            }

            recipeIngredientEntity.IngredientId = recipeIngredientUpdate.IngredientId;
            recipeIngredientEntity.Amount = recipeIngredientUpdate.Amount;
            recipeIngredientEntity.UpdatedDate = DateTime.UtcNow;

            await _recipeIngredientRepository.UpdateRecipeIngredient(recipeIngredientEntity);
        }

        public async Task DeleteRecipeIngredient(int id)
        {
            _logger.LogInformation("Usuwanie składnika przepisu o ID: {RecipeIngredientId}", id);
            try
            {
                var RecipeIngredientEntity = await _recipeIngredientRepository.GetRecipeIngredientById(id);
                if (RecipeIngredientEntity == null)
                {
                    _logger.LogWarning("RecipeIngredient o ID {Id} nie został znaleziony", id);
                    throw new KeyNotFoundException("RecipeIngredient not found");
                }

                RecipeIngredientEntity.IsDeleted = true;
                RecipeIngredientEntity.UpdatedDate = DateTime.UtcNow;

                await _recipeIngredientRepository.UpdateRecipeIngredient(RecipeIngredientEntity);
                _logger.LogDebug("Usunięto RecipeIngredient o ID {Id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas usuwania RecipeIngredient o ID {Id}", id);
                throw;
            }
        }

    }
}
