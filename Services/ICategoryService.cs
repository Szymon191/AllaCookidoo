using AllaCookidoo.Models;
using AllaCookidoo.Responses;

namespace AllaCookidoo.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponse>> GetCategories();
        Task<CategoryResponse> GetCategoryById(int id);
        Task AddCategory(CategoryRequest categoryCreation);
        Task UpdateCategory(int id, CategoryResponse categoryUpdate);
        Task DeleteCategory(int id);
    }
}
