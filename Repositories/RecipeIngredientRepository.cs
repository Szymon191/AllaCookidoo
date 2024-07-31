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
            _logger.LogInformation("Adding new recipe ingredient tp database");
            _context.RecipeIngredients.Add(recipeIngredient);
            await _context.SaveChangesAsync();
        }

        public async Task<RecipeIngredientEntity> GetRecipeIngredientById(int id)
        {
            _logger.LogInformation("Fetching recipe ingredient with ID: {RecipeIngredientId} from database", id);
            return await _context.RecipeIngredients
            .Include(ri => ri.Recipe)
            .Include(ri => ri.Ingredient)
            .FirstOrDefaultAsync(ri => ri.RecipeIngredientId == id && !ri.IsDeleted);
        }

        public async Task<IEnumerable<RecipeIngredientEntity>> GetRecipeIngredients()
        {
            _logger.LogInformation("Fetching all recipe ingredients from database");
            return await _context.RecipeIngredients.Where(x => !x.IsDeleted)
            .Include(ri => ri.Recipe)
            .Include(ri => ri.Ingredient)
            .Where(ri => !ri.IsDeleted)
            .ToListAsync();
        }

        public async Task UpdateRecipeIngredient(RecipeIngredientEntity recipeIngredient)
        {
            _logger.LogInformation("Updating recipe ingredient with ID: {RecipeIngredientId}", recipeIngredient.RecipeIngredientId);
            _context.Entry(recipeIngredient).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
