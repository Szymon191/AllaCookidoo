using AllaCookidoo.Models;
using AllaCookidoo.Responses;

namespace AllaCookidoo.Services
{
    public interface IIngredientService
    {
        Task<IEnumerable<IngredientResponse>> GetIngredients();
        Task<IngredientResponse> GetIngredientById(int id);
        Task AddIngredient(IngredientReguest IngredientCreation);
        Task UpdateIngredient(int id, IngredientResponse IngredientUpdate);
        Task DeleteIngredient(int id);
    }
}
