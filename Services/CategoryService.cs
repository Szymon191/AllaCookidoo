using AllaCookidoo.Entities;
using AllaCookidoo.Models;
using AllaCookidoo.Repositories;
using AllaCookidoo.Responses;

namespace AllaCookidoo.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }
        public async Task<IEnumerable<CategoryResponse>> GetCategories()
        {
            _logger.LogInformation("Fetching all categories");
            try
            {
                var categories = await _categoryRepository.GetCategory();
                _logger.LogDebug("Found {Count} categories", categories.Count());
                return categories.Select(category => new CategoryResponse
                {
                    CategoryId = category.Id,
                    Description = category.Description,
                    Name = category.Name,
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all categories");
                throw;
            }
        }

        public async Task<CategoryResponse> GetCategoryById(int id)
        {
            _logger.LogInformation("Fetching category with ID: {CategoryId}", id);
            try
            {
                var category = await _categoryRepository.GetCategoryById(id);
                if (category == null || category.IsDeleted)
                {
                    _logger.LogWarning("Category with ID {CategoryId} was not found or has been deleted", id);
                    return null;
                }

                _logger.LogDebug("Found category with ID {CategoryId}", id);
                return new CategoryResponse
                {
                    CategoryId = category.Id,
                    Description = category.Description,
                    Name = category.Name,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching category with ID {CategoryId}", id);
                throw;
            }
        }



        public async Task AddCategory(CategoryRequest categoryCreation)
        {
            _logger.LogInformation("Adding new category: {CategoryName}", categoryCreation.Name);
            try
            {
                var categoryEntity = new CategoryEntity
                {
                    Id = categoryCreation.CategoryId,
                    Name = categoryCreation.Name,
                    Description = categoryCreation.Description,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                };

                await _categoryRepository.AddCategory(categoryEntity);
                _logger.LogDebug("Added new category with ID {CategoryId}", categoryCreation.CategoryId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new category: {CategoryName}", categoryCreation.Name);
                throw;
            }
        }

        public async Task DeleteCategory(int id)
        {
            _logger.LogInformation("Deleting category with ID: {CategoryId}", id);
            try
            {
                var categoryEntity = await _categoryRepository.GetCategoryById(id);
                if (categoryEntity == null)
                {
                    _logger.LogWarning("Category with ID {CategoryId} was not found", id);
                    throw new KeyNotFoundException("Category not found");
                }

                categoryEntity.IsDeleted = true;
                categoryEntity.UpdatedDate = DateTime.UtcNow;

                await _categoryRepository.UpdateCategory(categoryEntity);
                _logger.LogDebug("Deleted category with ID {CategoryId}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category with ID {CategoryId}", id);
                throw;
            }

        }

        public async Task UpdateCategory(int id, CategoryResponse categoryUpdate)
        {
            _logger.LogInformation("Updating category with ID: {CategoryId}", id);
            try
            {
                var categoryEntity = await _categoryRepository.GetCategoryById(id);
                if (categoryEntity == null)
                {
                    _logger.LogWarning("Category with ID {CategoryId} was not found", id);
                    throw new KeyNotFoundException("Category not found");
                }

                categoryEntity.Name = categoryUpdate.Name;
                categoryEntity.Description = categoryUpdate.Description;
                categoryEntity.UpdatedDate = DateTime.UtcNow;

                await _categoryRepository.UpdateCategory(categoryEntity);
                _logger.LogDebug("Updated category with ID {CategoryId}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category with ID {CategoryId}", id);
                throw;
            }

        }
    }
}
