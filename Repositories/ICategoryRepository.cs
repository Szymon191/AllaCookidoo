using AllaCookidoo.Entities;

namespace AllaCookidoo.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryEntity>> GetCategory();
        Task<CategoryEntity> GetCategoryById(int id);
        Task AddCategory(CategoryEntity category);
        Task UpdateCategory(CategoryEntity category);
    }
}
