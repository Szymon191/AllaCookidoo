using AllaCookidoo.Entities;

namespace AllaCookidoo.Repositories
{
    public interface IRecipeIngredientRepository
    {
        Task<IEnumerable<RecipeIngredientEntity>> GetRecipeIngredients();
        Task<RecipeIngredientEntity> GetRecipeIngredientById(int id);
        Task AddRecipeIngredient(RecipeIngredientEntity recipeIngredient);
        Task UpdateRecipeIngredient(RecipeIngredientEntity recipeIngredient);
    }
}
