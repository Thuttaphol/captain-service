using Captain.DTOs;

namespace Captain.Services;

public interface ICategoryService
{
  Task<List<CategoryResponse>> GetCategoriesAsync(
    string userId,
    CancellationToken cancellationToken
  );
  Task<CategoryResponse?> GetCategoryByIdAsync(
    string userId,
    int categoryId,
    CancellationToken cancellationToken
  );
  Task<CategoryResponse?> GetCategoryByNameAsync(
    string userId,
    string categoryName,
    CancellationToken cancellationToken
  );
  Task<CategoryResponse> CreateCategoryAsync(
    string userId,
    CreateCategoryRequest request,
    CancellationToken cancellationToken
  );
  Task<CategoryResponse> UpdateCategoryAsync(
    string userId,
    UpdateCategoryRequest request,
    CancellationToken cancellationToken
  );
  Task<string> DeleteCategoryAsync(
    string userId,
    int categoryId,
    CancellationToken cancellationToken
  );
}
