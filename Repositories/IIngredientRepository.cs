using AllaCookidoo.Entities;

namespace AllaCookidoo.Repositories
{
    public interface IIngredientRepository
    {
        Task<IEnumerable<IngredientEntity>> GetIngredients();
        Task<IngredientEntity> GetIngredientById(int id);
        Task AddIngredient(IngredientEntity Ingredient);
        Task UpdateIngredient(IngredientEntity Ingredient);
    }
}
