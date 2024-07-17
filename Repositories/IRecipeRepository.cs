using AllaCookidoo.Entities;

namespace AllaCookidoo.Repositories
{
    public interface IRecipeRepository
    {
        Task<IEnumerable<RecipeEntity>> GetRecipes();
        Task<IEnumerable<RecipeEntity>> GetRecipesFromCategory(int categoryId);
        Task<RecipeEntity> GetRecipeById(int id);
        Task AddRecipe(RecipeEntity recipe);
        Task UpdateRecipe(RecipeEntity recipe);
    }
}
