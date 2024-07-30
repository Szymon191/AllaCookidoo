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
            _logger.LogInformation("dodawanie nowego skladniku: {name}", IngredientCreation.Name);
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
                _logger.LogDebug("Dodano nowego skladniku o ID {IngredientId}", IngredientCreation.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas dodawania nowego skladniku: {name}", IngredientCreation.Name);
                throw;
            }
        }

        public async Task DeleteIngredient(int id)
        {
            _logger.LogInformation("usuwanie skladniku o ID: {Id}", id);
            try
            {
                var ingredientEntity = await _ingredientRepository.GetIngredientById(id);
                if (ingredientEntity == null)
                {
                    _logger.LogWarning("skladnik o ID {Id} nie została znaleziona", id);
                    throw new KeyNotFoundException("Ingredient not found");
                }

                ingredientEntity.IsDeleted = true;
                ingredientEntity.UpdatedDate = DateTime.UtcNow;

                await _ingredientRepository.UpdateIngredient(ingredientEntity);
                _logger.LogDebug("Usunięto skladnik o ID {Id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas usuwania skladniku o ID {Id}", id);
                throw;
            }
        }

        public async Task<IngredientResponse> GetIngredientById(int id)
        {
            _logger.LogInformation("pobieranie skladniku o ID: {Id}", id);
            try
            {
                var ingredient = await _ingredientRepository.GetIngredientById(id);
                if (ingredient == null || ingredient.IsDeleted)
                {
                    _logger.LogWarning("skladnik o ID {Id} nie została znaleziona lub została usunięta", id);
                    return null;
                }

                _logger.LogDebug("Znaleziono skladnik o ID {Id}", id);
                return new IngredientResponse
                {
                    Id = ingredient.Id,
                    Name = ingredient.Name,       
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas pobierania skladniku o ID {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<IngredientResponse>> GetIngredients()
        {
            _logger.LogInformation("pobieranie wszystkich skladnikow");
            try
            {
                var ingredient = await _ingredientRepository.GetIngredients();
                _logger.LogDebug("Znaleziono {Count} skladnikow", ingredient.Count());
                return ingredient.Select(ingredient => new IngredientResponse
                {
                    Id = ingredient.Id,
                    Name = ingredient.Name,
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas pobierania wszystkich skladnikow");
                throw;
            }
        }

        public async Task UpdateIngredient(int id, IngredientResponse IngredientUpdate)
        {
            _logger.LogInformation("aktualizacja skladnikow o ID: {Id}", id);
            try
            {
                var ingredientEntity = await _ingredientRepository.GetIngredientById(id);
                if (ingredientEntity == null)
                {
                    _logger.LogWarning("skladniki o ID {Id} nie została znaleziona", id);
                    throw new KeyNotFoundException("Ingredient not found");
                }

                ingredientEntity.Name = IngredientUpdate.Name;
                ingredientEntity.UpdatedDate = DateTime.UtcNow;

                await _ingredientRepository.UpdateIngredient(ingredientEntity);
                _logger.LogDebug("Zaktualizowano skladnik o ID {Id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas aktualizacji skladniku o ID {Id}", id);
                throw;
            }
        }
    }
}
