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
            _logger.LogInformation("Fetching all categories from database");
            return await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<CategoryEntity> GetCategoryById(int id)
        {
            _logger.LogInformation("Fetching category with ID: {CategoryId} from database", id);
            return await _context.Categories.FindAsync(id);
        }

        public async Task AddCategory(CategoryEntity category)
        {
            _logger.LogInformation("Adding new category to database");
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateCategory(CategoryEntity category)
        {
            _logger.LogInformation("Updating category with ID: {CategoryId} in database", category.Id);
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
