using AllaCookidoo.Database;
using AllaCookidoo.Entities;
using Microsoft.EntityFrameworkCore;

namespace AllaCookidoo.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AllaCookidoDatabaseContext _context;
        private readonly ILogger<CategoryRepository> _logger;

        public CategoryRepository(AllaCookidoDatabaseContext context, ILogger<CategoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<CategoryEntity>> GetCategory()
        {
            _logger.LogInformation("Pobieranie wszystkich kategorii z bazy danych");
            return await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<CategoryEntity> GetCategoryById(int id)
        {
            _logger.LogInformation("Pobieranie kategorii o ID: {RecipeId} z bazy danych", id);
            return await _context.Categories.FindAsync(id);
        }

        public async Task AddCategory(CategoryEntity category)
        {
            _logger.LogInformation("Dodawanie nowej kategorii do bazy danych");
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateCategory(CategoryEntity category)
        {
            _logger.LogInformation("Aktualizacja kategorii o ID: {RecipeId} w bazie danych", category.Id);
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
