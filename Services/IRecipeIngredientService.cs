using AllaCookidoo.Models;
using AllaCookidoo.Responses;

namespace AllaCookidoo.Services
{
    public interface IRecipeIngredientService
    {
        Task<IEnumerable<RecipeIngredientResponse>> GetRecipeIngredients();
        Task<RecipeIngredientResponse> GetRecipeIngredientById(int id);
        Task AddRecipeIngredient(RecipeIngredientRequest recipeIngredientCreation);
        Task UpdateRecipeIngredient(int id, RecipeIngredientResponse recipeIngredientUpdate);
        Task DeleteRecipeIngredient(int id);
    }
}
