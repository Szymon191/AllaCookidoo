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
            _logger.LogInformation("pobieranie wszystkich kategorii");
            try
            {
                var categories = await _categoryRepository.GetCategory();
                _logger.LogDebug("Znaleziono {Count} kategorii", categories.Count());
                return categories.Select(category => new CategoryResponse
                {
                    CategoryId = category.Id,
                    Description = category.Description,
                    Name = category.Name,
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas pobierania wszystkich kategorii");
                throw;
            }
        }

        public async Task<CategoryResponse> GetCategoryById(int id)
        {
            _logger.LogInformation("pobieranie kategorii o ID: {CategoryId}", id);
            try
            {
                var category = await _categoryRepository.GetCategoryById(id);
                if (category == null || category.IsDeleted)
                {
                    _logger.LogWarning("Kategoria o ID {CategoryId} nie została znaleziona lub została usunięta", id);
                    return null;
                }

                _logger.LogDebug("Znaleziono kategorię o ID {CategoryId}", id);
                return new CategoryResponse
                {
                    CategoryId = category.Id,
                    Description = category.Description,
                    Name = category.Name,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas pobierania kategorii o ID {CategoryId}", id);
                throw;
            }
        }



        public async Task AddCategory(CategoryRequest categoryCreation)
        {
            _logger.LogInformation("dodawanie nowej kategorii: {CategoryName}", categoryCreation.Name);
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
                _logger.LogDebug("Dodano nową kategorię o ID {CategoryId}", categoryCreation.CategoryId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas dodawania nowej kategorii: {CategoryName}", categoryCreation.Name);
                throw;
            }
        }

        public async Task DeleteCategory(int id)
        {
            _logger.LogInformation("usuwanie kategorii o ID: {CategoryId}", id);
            try
            {
                var categoryEntity = await _categoryRepository.GetCategoryById(id);
                if (categoryEntity == null)
                {
                    _logger.LogWarning("Kategoria o ID {CategoryId} nie została znaleziona", id);
                    throw new KeyNotFoundException("Category not found");
                }

                categoryEntity.IsDeleted = true;
                categoryEntity.UpdatedDate = DateTime.UtcNow;

                await _categoryRepository.UpdateCategory(categoryEntity);
                _logger.LogDebug("Usunięto kategorię o ID {CategoryId}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas usuwania kategorii o ID {CategoryId}", id);
                throw;
            }

        }

        public async Task UpdateCategory(int id, CategoryResponse categoryUpdate)
        {
            _logger.LogInformation("aktualizacja kategorii o ID: {CategoryId}", id);
            try
            {
                var categoryEntity = await _categoryRepository.GetCategoryById(id);
                if (categoryEntity == null)
                {
                    _logger.LogWarning("Kategoria o ID {CategoryId} nie została znaleziona", id);
                    throw new KeyNotFoundException("Category not found");
                }

                categoryEntity.Name = categoryUpdate.Name;
                categoryEntity.Description = categoryUpdate.Description;
                categoryEntity.UpdatedDate = DateTime.UtcNow;

                await _categoryRepository.UpdateCategory(categoryEntity);
                _logger.LogDebug("Zaktualizowano kategorię o ID {CategoryId}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas aktualizacji kategorii o ID {CategoryId}", id);
                throw;
            }

        }
    }
}
