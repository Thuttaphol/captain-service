using Captain.Data;
using Captain.DTOs;
using Captain.Models;
using Microsoft.EntityFrameworkCore;

namespace Captain.Services;

public class CategoryService(MoneyDbContext moneyContext) : ICategoryService
{
  private readonly MoneyDbContext _moneyContext = moneyContext;

  public async Task<List<CategoryResponse>> GetCategoriesAsync(CancellationToken cancellationToken)
  {
    List<CategoryResponse> response = await _moneyContext
      .Categories.AsNoTracking()
      .Select(category => new CategoryResponse { Id = category.Id, Name = category.Name })
      .ToListAsync(cancellationToken);

    return response;
  }

  public async Task<CategoryResponse?> GetCategoryByIdAsync(
    int categoryId,
    CancellationToken cancellationToken
  )
  {
    CategoryResponse? category = await _moneyContext
      .Categories.AsNoTracking()
      .Where(category => category.Id == categoryId)
      .Select(category => new CategoryResponse { Id = category.Id, Name = category.Name })
      .FirstOrDefaultAsync(cancellationToken);

    if (category is null)
    {
      return null;
    }

    return category;
  }

  public async Task<CategoryResponse?> GetCategoryByNameAsync(
    string categoryName,
    CancellationToken cancellationToken
  )
  {
    CategoryResponse? category = await _moneyContext
      .Categories.AsNoTracking()
      .Where(category => category.Name == categoryName)
      .Select(category => new CategoryResponse { Id = category.Id, Name = category.Name })
      .FirstOrDefaultAsync(cancellationToken);

    if (category is null)
    {
      return null;
    }

    return category;
  }

  public async Task<CategoryResponse> CreateCategoryAsync(
    CreateCategoryRequest request,
    CancellationToken cancellationToken
  )
  {
    bool isCategoryExist = await _moneyContext.Categories.AnyAsync(
      category => category.Name == request.Name,
      cancellationToken
    );

    if (isCategoryExist)
    {
      throw new Exception("The Category already exists");
    }

    var category = new Category { Name = request.Name };

    _moneyContext.Categories.Add(category);
    await _moneyContext.SaveChangesAsync(cancellationToken);

    var savedCategory =
      await GetCategoryByNameAsync(categoryName: request.Name, cancellationToken)
      ?? throw new InvalidOperationException("The transaction was created but could not be read.");

    return savedCategory;
  }

  public async Task<CategoryResponse> UpdateCategoryAsync(
    UpdateCategoryRequest request,
    CancellationToken cancellationToken
  )
  {
    var category =
      await _moneyContext.Categories.FirstOrDefaultAsync(
        category => category.Id == request.Id,
        cancellationToken
      ) ?? throw new Exception("Category not found");

    category.Name = request.Name;

    await _moneyContext.SaveChangesAsync(cancellationToken);

    var updatedCategory =
      await GetCategoryByIdAsync(categoryId: category.Id, cancellationToken)
      ?? throw new InvalidOperationException("The transaction was created but could not be read.");

    return updatedCategory;
  }

  public async Task<bool> DeleteCategoryAsync(int categoryId, CancellationToken cancellationToken)
  {
    var category =
      await _moneyContext.Categories.FirstOrDefaultAsync(
        category => category.Id == categoryId,
        cancellationToken
      ) ?? throw new Exception("Category not found");

    if (category is null)
    {
      return false;
    }

    _moneyContext.Remove(category);

    await _moneyContext.SaveChangesAsync(cancellationToken);

    return true;
  }
}
