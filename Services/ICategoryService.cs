using Captain.DTOs;

namespace Captain.Services;

public interface ICategoryService
{
  Task<List<CategoryResponse>> GetAllCategory(CancellationToken cancellationToken);
  Task<CategoryResponse?> GetCategoryById(int CategoryId, CancellationToken cancellationToken);
  Task<CategoryResponse> CreateCategory(
    CreateCategoryRequest request,
    CancellationToken cancellationToken
  );
  Task<CategoryResponse> UpdateCategory(
    UpdateCategoryRequest request,
    CancellationToken cancellationToken
  );
  Task<bool> DeleteCategory(int categoryId, CancellationToken cancellationToken);
}
