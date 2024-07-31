using AllaCookidoo.Entities;
using AllaCookidoo.Models;
using AllaCookidoo.Repositories;
using AllaCookidoo.Responses;

namespace AllaCookidoo.Services
{
    public class IngredientService : IIngredientService
    {

        private readonly IIngredientRepository _ingredientRepository;
        private readonly ILogger<IngredientService> _logger;

        public IngredientService(IIngredientRepository ingredientRepository, ILogger<IngredientService> logger)
        {
            _ingredientRepository = ingredientRepository;
            _logger = logger;
        }

        public async Task AddIngredient(IngredientReguest IngredientCreation)
        {
            _logger.LogInformation("Adding new ingredient: {name}", IngredientCreation.Name);
            try
            {
                var IngredientEntity = new IngredientEntity
                {
                    Id = IngredientCreation.Id,
                    Name = IngredientCreation.Name,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                };

                await _ingredientRepository.AddIngredient(IngredientEntity);
                _logger.LogDebug("Added new ingredient with ID {ingredientId}", IngredientCreation.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new ingredient: {name}", IngredientCreation.Name);
                throw;
            }
        }

        public async Task DeleteIngredient(int id)
        {
            _logger.LogInformation("Deleting ingredient with ID: {Id}", id);
            try
            {
                var ingredientEntity = await _ingredientRepository.GetIngredientById(id);
                if (ingredientEntity == null)
                {
                    _logger.LogWarning("Ingredient with ID {Id} was not found", id);
                    throw new KeyNotFoundException("Ingredient not found");
                }

                ingredientEntity.IsDeleted = true;
                ingredientEntity.UpdatedDate = DateTime.UtcNow;

                await _ingredientRepository.UpdateIngredient(ingredientEntity);
                _logger.LogDebug("Deleted ingredient with ID {Id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting ingredient with ID {Id}", id);
                throw;
            }
        }

        public async Task<IngredientResponse> GetIngredientById(int id)
        {
            _logger.LogInformation("Fetching ingredient with ID: {Id}", id);
            try
            {
                var ingredient = await _ingredientRepository.GetIngredientById(id);
                if (ingredient == null || ingredient.IsDeleted)
                {
                    _logger.LogWarning("Ingredient with ID {Id} was not found or has been deleted", id);
                    return null;
                }

                _logger.LogDebug("Found ingredient with ID {Id}", id);
                return new IngredientResponse
                {
                    Id = ingredient.Id,
                    Name = ingredient.Name,       
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching ingredient with ID {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<IngredientResponse>> GetIngredients()
        {
            _logger.LogInformation("Fetching all ingredients");
            try
            {
                var ingredient = await _ingredientRepository.GetIngredients();
                _logger.LogDebug("Found {Count} ingredients", ingredient.Count());
                return ingredient.Select(ingredient => new IngredientResponse
                {
                    Id = ingredient.Id,
                    Name = ingredient.Name,
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all ingredients");
                throw;
            }
        }

        public async Task UpdateIngredient(int id, IngredientResponse IngredientUpdate)
        {
            _logger.LogInformation("Updating ingredient with ID: {Id}", id);
            try
            {
                var ingredientEntity = await _ingredientRepository.GetIngredientById(id);
                if (ingredientEntity == null)
                {
                    _logger.LogWarning("Ingredient with ID {Id} was not found", id);
                    throw new KeyNotFoundException("Ingredient not found");
                }

                ingredientEntity.Name = IngredientUpdate.Name;
                ingredientEntity.UpdatedDate = DateTime.UtcNow;

                await _ingredientRepository.UpdateIngredient(ingredientEntity);
                _logger.LogDebug("Updated ingredient with ID {Id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating ingredient with ID {Id}", id);
                throw;
            }
        }
    }
}
