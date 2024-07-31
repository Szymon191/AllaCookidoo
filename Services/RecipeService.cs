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
        private readonly ICategoryRepository _categoryRepository;
        public RecipeService(IRecipeRepository recipeRepository, ILogger<RecipeService> logger, ICategoryRepository categoryRepository)
        {
            _recipeRepository = recipeRepository;
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<RecipeResponse>> GetRecipes()
        {
            _logger.LogInformation("Fetching all recipes");
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

                _logger.LogDebug("Found {Count} recipes", response.Count);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all recipes");
                throw;
            }
        }

        public async Task<IEnumerable<RecipeResponse>> GetRecipesFromCategory(int categoryId)
        {
            _logger.LogInformation("Fetching recipes for category: {CategoryId}", categoryId);
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

                _logger.LogDebug("Found {Count} recipes for category {CategoryId}", response.Count, categoryId);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching recipes for category {CategoryId}", categoryId);
                throw;
            }
        }

        public async Task<RecipeResponse> GetRecipeById(int id)
        {
            _logger.LogInformation("Fetching recipe with ID: {RecipeId}", id);
            try
            {
                var recipe = await _recipeRepository.GetRecipeById(id);
                if (recipe == null || recipe.IsDeleted)
                {
                    _logger.LogWarning("Recipe with ID {RecipeId} was not found or has been deleted", id);
                    return null;
                }

                var response = new RecipeResponse
                {
                    Name = recipe.Name,
                    CategoryId = recipe.CategoryId,
                    Description = recipe.Description,
                    RecipeId = recipe.Id,
                };

                _logger.LogDebug("Found recipe with ID {RecipeId}: {RecipeName}", id, response.Name);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching recipe with ID {RecipeId}", id);
                throw;
            }
        }

        public async Task AddRecipe(RecipeRequest recipeCreation)
        {
            _logger.LogInformation("Adding new recipe: {RecipeName}", recipeCreation.Name);

            var categoryExists = await _categoryRepository.GetCategoryById(recipeCreation.CategoryId);
            if (categoryExists == null)
            {
                _logger.LogWarning("Category with ID {CategoryId} does not exist", recipeCreation.CategoryId);
                throw new ArgumentException($"Category with ID {recipeCreation.CategoryId} does not exist.");
            }
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
                _logger.LogDebug("Added new recipe with ID {RecipeId}", recipeEntity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new recipe: {RecipeName}", recipeCreation.Name);
                throw;
            }
        }

        public async Task UpdateRecipe(int id, RecipeDetailsResponse recipeUpdate)
        {
            _logger.LogInformation("Updating recipe with ID: {RecipeId}", id);
            try
            {
                var recipeEntity = await _recipeRepository.GetRecipeById(id);
                if (recipeEntity == null)
                {
                    _logger.LogWarning("Recipe with ID {RecipeId} was not found", id);
                    throw new KeyNotFoundException("Recipe not found");
                }

                recipeEntity.Name = recipeUpdate.Name;
                recipeEntity.CategoryId = recipeUpdate.CategoryId;
                recipeEntity.CookTime = recipeUpdate.CookTime;
                recipeEntity.Description = recipeUpdate.Description;
                recipeEntity.Instruction = recipeUpdate.Instruction;
                recipeEntity.UpdatedDate = DateTime.UtcNow;

                await _recipeRepository.UpdateRecipe(recipeEntity);
                _logger.LogDebug("Updated recipe with ID {RecipeId}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating recipe with ID {RecipeId}", id);
                throw;
            }
        }

        public async Task DeleteRecipe(int id)
        {
            _logger.LogInformation("Deleting recipe with ID: {RecipeId}", id);
            try
            {
                var recipeEntity = await _recipeRepository.GetRecipeById(id);
                if (recipeEntity == null)
                {
                    _logger.LogWarning("Recipe with ID {RecipeId} was not found", id);
                    throw new KeyNotFoundException("Recipe not found");
                }

                recipeEntity.IsDeleted = true;
                recipeEntity.UpdatedDate = DateTime.UtcNow;

                await _recipeRepository.UpdateRecipe(recipeEntity);
                _logger.LogDebug("Deleted recipe with ID {RecipeId}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting recipe with ID {RecipeId}", id);
                throw;
            }
        }

        public async Task<RecipeDetailsResponse> GetRecipeDetailsById(int id)
        {
            _logger.LogInformation("Fetching recipe details with ID: {RecipeId}", id);
            try
            {
                var recipe = await _recipeRepository.GetRecipeById(id);
                if (recipe == null || recipe.IsDeleted)
                {
                    _logger.LogWarning("Recipe with ID {RecipeId} was not found or has been deleted", id);
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

                _logger.LogDebug("Found recipe details with ID {RecipeId}: {RecipeName}", id, response.Name);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching recipe details with ID {RecipeId}", id);
                throw;
            }
        }
    }
}
