using AllaCookidoo.Database;
using AllaCookidoo.Entities;
using Microsoft.EntityFrameworkCore;

namespace AllaCookidoo.Repositories
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly AllaCookidoDatabaseContext _context;
        private readonly ILogger<IngredientRepository> _logger;

        public IngredientRepository(AllaCookidoDatabaseContext context, ILogger<IngredientRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddIngredient(IngredientEntity Ingredient)
        {
            _logger.LogInformation("Adding new ingredient to database");
            _context.Ingredients.Add(Ingredient);
            await _context.SaveChangesAsync();
        }

        public async Task<IngredientEntity> GetIngredientById(int id)
        {
            _logger.LogInformation($"Fetching ingredient with ID: {id} from database");
            return await _context.Ingredients.FindAsync(id);
        }

        public async Task<IEnumerable<IngredientEntity>> GetIngredients()
        {
            _logger.LogInformation("Fetching all ingredients from database");
            return await _context.Ingredients.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task UpdateIngredient(IngredientEntity Ingredient)
        {
            _logger.LogInformation($"Updating ingredient with ID: {Ingredient.Id} in database");
            _context.Entry(Ingredient).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
