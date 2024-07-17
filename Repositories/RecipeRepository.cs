using AllaCookidoo.Database;
using AllaCookidoo.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AllaCookidoo.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly AllaCookidoDatabaseContext _context;
        private readonly ILogger<RecipeRepository> _logger;

        public RecipeRepository(AllaCookidoDatabaseContext context, ILogger<RecipeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<RecipeEntity>> GetRecipes()
        {
            _logger.LogInformation("Pobieranie wszystkich przepisów z bazy danych");
            return await _context.Recipes.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<RecipeEntity>> GetRecipesFromCategory(int categoryId)
        {
            _logger.LogInformation("Pobieranie przepisów z kategorii o ID: {CategoryId}", categoryId);
            return await _context.Recipes.Where(x => !x.IsDeleted && x.CategoryId == categoryId).ToListAsync();
        }

        public async Task<RecipeEntity> GetRecipeById(int id)
        {
            _logger.LogInformation("Pobieranie przepisu o ID: {RecipeId} z bazy danych", id);
            return await _context.Recipes.FindAsync(id);
        }

        public async Task AddRecipe(RecipeEntity recipe)
        {
            try
            {
                _logger.LogInformation("Dodawanie nowego przepisu do bazy danych");

                _context.Recipes.Add(recipe);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("blad podczas dodawania nowego przepisu {ex}", ex);
            }


            
        }

        public async Task UpdateRecipe(RecipeEntity recipe)
        {
            _logger.LogInformation("Aktualizacja przepisu o ID: {RecipeId} w bazie danych", recipe.Id);
            _context.Entry(recipe).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
