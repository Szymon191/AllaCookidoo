using AllaCookidoo.Models;
using AllaCookidoo.Responses;

namespace AllaCookidoo.Services
{
    public interface IRecipeService
    {
        Task<IEnumerable<RecipeResponse>> GetRecipes();
        Task<IEnumerable<RecipeResponse>> GetRecipesFromCategory(int categoryId);
        Task<RecipeResponse> GetRecipeById(int id);
        Task<RecipeDetailsResponse> GetRecipeDetailsById(int id);
        Task AddRecipe(RecipeRequest recipeCreation);
        Task UpdateRecipe(int id, RecipeDetailsResponse recipeUpdate);
        Task DeleteRecipe(int id);
    }
}
