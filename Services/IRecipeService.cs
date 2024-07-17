using AllaCookidoo.Models;
using AllaCookidoo.Responses;

namespace AllaCookidoo.Services
{
    public interface IRecipeService
    {
        Task<IEnumerable<RecipeResponse>> GetRecipes();
        Task<IEnumerable<RecipeResponse>> GetRecipesFromCategory(int categoryId);
        Task<RecipeResponse> GetRecipeById(int id);
        Task AddRecipe(RecipeRequest recipeCreation);
        Task UpdateRecipe(int id, RecipeResponse recipeUpdate);
        Task DeleteRecipe(int id);
    }
}
