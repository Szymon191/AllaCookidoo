using AllaCookidoo.Database;
using AllaCookidoo.Entities;
using Microsoft.EntityFrameworkCore;

namespace AllaCookidoo.Repositories
{
    public class RecipeIngredientRepository : IRecipeIngredientRepository
    {
        private readonly AllaCookidoDatabaseContext _context;
        private readonly ILogger<RecipeIngredientRepository> _logger;

        public RecipeIngredientRepository(AllaCookidoDatabaseContext context, ILogger<RecipeIngredientRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddRecipeIngredient(RecipeIngredientEntity recipeIngredient)
        {
            _logger.LogInformation("Dodawanie nowego składnika przepisu do bazy danych");
            _context.RecipeIngredients.Add(recipeIngredient);
            await _context.SaveChangesAsync();
        }

        public async Task<RecipeIngredientEntity> GetRecipeIngredientById(int id)
        {
            _logger.LogInformation("Pobieranie składnika przepisu o ID: {RecipeIngredientId} z bazy danych", id);
            return await _context.RecipeIngredients
            .Include(ri => ri.Recipe)
            .Include(ri => ri.Ingredient)
            .FirstOrDefaultAsync(ri => ri.RecipeIngredientId == id && !ri.IsDeleted);
        }

        public async Task<IEnumerable<RecipeIngredientEntity>> GetRecipeIngredients()
        {
            _logger.LogInformation("Pobieranie wszystkich składników przepisu z bazy danych");
            return await _context.RecipeIngredients.Where(x => !x.IsDeleted)
            .Include(ri => ri.Recipe)
            .Include(ri => ri.Ingredient)
            .Where(ri => !ri.IsDeleted)
            .ToListAsync();
        }

        public async Task UpdateRecipeIngredient(RecipeIngredientEntity recipeIngredient)
        {
            _logger.LogInformation("Aktualizacja składnika przepisu o ID: {RecipeIngredientId}", recipeIngredient.RecipeIngredientId);
            _context.Entry(recipeIngredient).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
