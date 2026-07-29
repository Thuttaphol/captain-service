using Captain.DTOs;

namespace Captain.Services;

public interface ICategoryService
{
  Task<List<CategoryResponse>> GetCategoriesAsync(CancellationToken cancellationToken);
  Task<CategoryResponse?> GetCategoryByIdAsync(int CategoryId, CancellationToken cancellationToken);
  Task<CategoryResponse> CreateCategoryAsync(
    CreateCategoryRequest request,
    CancellationToken cancellationToken
  );
  Task<CategoryResponse> UpdateCategoryAsync(
    UpdateCategoryRequest request,
    CancellationToken cancellationToken
  );
  Task<bool> DeleteCategoryAsync(int categoryId, CancellationToken cancellationToken);
}
