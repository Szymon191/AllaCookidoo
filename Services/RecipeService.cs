using AllaCookidoo.Entities;
using AllaCookidoo.Models;
using AllaCookidoo.Repositories;
using AllaCookidoo.Responses;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace AllaCookidoo.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly ILogger<RecipeService> _logger;
        public RecipeService(IRecipeRepository recipeRepository, ILogger<RecipeService> logger)
        {
            _recipeRepository = recipeRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<RecipeResponse>> GetRecipes()
        {
            _logger.LogInformation("Pobieranie wszystkich przepisów");
            try
            {
                var recipes = await _recipeRepository.GetRecipes();
                var response = recipes.Select(recipe => new RecipeResponse
                {
                    CategoryId = recipe.CategoryId,
                    RecipeId = recipe.Id,
                    Description = recipe.Description,
                    Name = recipe.Name
                }).ToList();

                _logger.LogDebug("Znaleziono {Count} przepisów", response.Count);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas pobierania wszystkich przepisów");
                throw;
            }
        }

        public async Task<IEnumerable<RecipeResponse>> GetRecipesFromCategory(int categoryId)
        {
            _logger.LogInformation("Pobieranie przepisów dla kategorii: {CategoryId}", categoryId);
            try
            {
                var recipes = await _recipeRepository.GetRecipesFromCategory(categoryId);
                var response = recipes.Select(recipe => new RecipeResponse
                {
                    CategoryId = recipe.CategoryId,
                    RecipeId = recipe.Id,
                    Description = recipe.Description,
                    Name = recipe.Name
                }).ToList();

                _logger.LogDebug("Znaleziono {Count} przepisów dla kategorii {CategoryId}", response.Count, categoryId);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas pobierania przepisów dla kategorii {CategoryId}", categoryId);
                throw;
            }
        }

        public async Task<RecipeResponse> GetRecipeById(int id)
        {
            _logger.LogInformation("Pobieranie przepisu o ID: {RecipeId}", id);
            try
            {
                var recipe = await _recipeRepository.GetRecipeById(id);
                if (recipe == null || recipe.IsDeleted)
                {
                    _logger.LogWarning("Przepis o ID {RecipeId} nie został znaleziony lub został usunięty", id);
                    return null;
                }

                var response = new RecipeResponse
                {
                    Name = recipe.Name,
                    CategoryId = recipe.CategoryId,
                    Description = recipe.Description,
                    RecipeId = recipe.Id,
                };

                _logger.LogDebug("Znaleziono przepis o ID {RecipeId}: {RecipeName}", id, response.Name);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas pobierania przepisu o ID {RecipeId}", id);
                throw;
            }
        }

        public async Task AddRecipe(RecipeRequest recipeCreation)
        {
            _logger.LogInformation("Dodawanie nowego przepisu: {RecipeName}", recipeCreation.Name);
            try
            {
                var recipeEntity = new RecipeEntity
                {
                    Name = recipeCreation.Name,
                    CategoryId = recipeCreation.CategoryId,
                    IsDeleted = false,
                    UpdatedDate = DateTime.UtcNow,
                    CreatedDate = DateTime.UtcNow,
                    Id = recipeCreation.RecipeId,
                    CookTime = recipeCreation.CookTime,
                    Description = recipeCreation.Description,
                    Instruction = recipeCreation.Instruction,
                };

                await _recipeRepository.AddRecipe(recipeEntity);
                _logger.LogDebug("Dodano nowy przepis o ID {RecipeId}", recipeEntity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas dodawania nowego przepisu: {RecipeName}", recipeCreation.Name);
                throw;
            }
        }

        public async Task UpdateRecipe(int id, RecipeDetailsResponse recipeUpdate)
        {
            _logger.LogInformation("Aktualizacja przepisu o ID: {RecipeId}", id);
            try
            {
                var recipeEntity = await _recipeRepository.GetRecipeById(id);
                if (recipeEntity == null)
                {
                    _logger.LogWarning("Przepis o ID {RecipeId} nie został znaleziony", id);
                    throw new KeyNotFoundException("Recipe not found");
                }

                recipeEntity.Name = recipeUpdate.Name;
                recipeEntity.CategoryId = recipeUpdate.CategoryId;
                recipeEntity.CookTime = recipeUpdate.CookTime;
                recipeEntity.Description = recipeUpdate.Description;
                recipeEntity.Instruction = recipeUpdate.Instruction;
                recipeEntity.UpdatedDate = DateTime.UtcNow;

                await _recipeRepository.UpdateRecipe(recipeEntity);
                _logger.LogDebug("Zaktualizowano przepis o ID {RecipeId}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas aktualizacji przepisu o ID {RecipeId}", id);
                throw;
            }
        }

        public async Task DeleteRecipe(int id)
        {
            _logger.LogInformation("Usuwanie przepisu o ID: {RecipeId}", id);
            try
            {
                var recipeEntity = await _recipeRepository.GetRecipeById(id);
                if (recipeEntity == null)
                {
                    _logger.LogWarning("Przepis o ID {RecipeId} nie został znaleziony", id);
                    throw new KeyNotFoundException("Recipe not found");
                }

                recipeEntity.IsDeleted = true;
                recipeEntity.UpdatedDate = DateTime.UtcNow;

                await _recipeRepository.UpdateRecipe(recipeEntity);
                _logger.LogDebug("Usunięto przepis o ID {RecipeId}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas usuwania przepisu o ID {RecipeId}", id);
                throw;
            }
        }

        public async Task<RecipeDetailsResponse> GetRecipeDetailsById(int id)
        {
            _logger.LogInformation("Pobieranie szczegolow przepisu o ID: {RecipeId}", id);
            try
            {
                var recipe = await _recipeRepository.GetRecipeById(id);
                if (recipe == null || recipe.IsDeleted)
                {
                    _logger.LogWarning("Przepis o ID {RecipeId} nie został znaleziony lub został usunięty", id);
                    return null;
                }

                var response = new RecipeDetailsResponse
                {
                    Name = recipe.Name,
                    CategoryId = recipe.CategoryId,
                    Description = recipe.Description,
                    RecipeId = recipe.Id,
                    CookTime = recipe.CookTime,
                    Instruction = recipe.Instruction,
                    Feedbacks = recipe.Feedbacks.Select(f => new FeedbackResponse
                    {
                        Id = f.Id,
                        Evaluation = f.Evaluation,
                        Opinion = f.Opinion,
                        RecipeId = f.RecipeId
                    }).ToList(),
                    RecipeIngredients = recipe.RecipeIngredients.Select(ri => new RecipeIngredientResponse
                    {
                        RecipeIngredientId = ri.RecipeIngredientId,
                        IngredientId = ri.IngredientId,
                        Amount = ri.Amount,
                        IngredientName = ri.Ingredient.Name
                    }).ToList()
                };

                _logger.LogDebug("Znaleziono szczegoly przepisu o ID {RecipeId}: {RecipeName}", id, response.Name);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas pobierania szczegolow przepisu o ID {RecipeId}", id);
                throw;
            }
        }
    }
}
