using Captain.Data;
using Captain.DTOs;
using Captain.Exceptions;
using Captain.Models;
using Microsoft.EntityFrameworkCore;

namespace Captain.Services;

public class CategoryService(MoneyDbContext moneyContext) : ICategoryService
{
  private readonly MoneyDbContext _moneyContext = moneyContext;

  public async Task<List<CategoryResponse>> GetCategoriesAsync(
    string userId,
    CancellationToken cancellationToken
  )
  {
    List<CategoryResponse> response = await _moneyContext
      .Categories.AsNoTracking()
      .Where(category => category.AppUserId == userId)
      .Select(category => new CategoryResponse
      {
        Id = category.Id,
        Name = category.Name,
        TransactionType = category.TransacionType,
      })
      .ToListAsync(cancellationToken);

    return response;
  }

  public async Task<CategoryResponse?> GetCategoryByIdAsync(
    string userId,
    int categoryId,
    CancellationToken cancellationToken
  )
  {
    CategoryResponse? category = await _moneyContext
      .Categories.AsNoTracking()
      .Where(category => category.Id == categoryId && category.AppUserId == userId)
      .Select(category => new CategoryResponse
      {
        Id = category.Id,
        Name = category.Name,
        TransactionType = category.TransacionType,
      })
      .FirstOrDefaultAsync(cancellationToken);

    return category;
  }

  public async Task<CategoryResponse?> GetCategoryByNameAsync(
    string userId,
    string categoryName,
    CancellationToken cancellationToken
  )
  {
    CategoryResponse? category = await _moneyContext
      .Categories.AsNoTracking()
      .Where(category => category.Name == categoryName && category.AppUserId == userId)
      .Select(category => new CategoryResponse
      {
        Id = category.Id,
        Name = category.Name,
        TransactionType = category.TransacionType,
      })
      .FirstOrDefaultAsync(cancellationToken);

    return category;
  }

  public async Task<CategoryResponse> CreateCategoryAsync(
    string userId,
    CreateCategoryRequest request,
    CancellationToken cancellationToken
  )
  {
    bool isCategoryExist = await _moneyContext.Categories.AnyAsync(
      category => category.Name == request.Name & category.AppUserId == userId,
      cancellationToken
    );

    if (isCategoryExist)
    {
      throw new ConflictException("Category already exists");
    }

    var category = new Category { Name = request.Name, AppUserId = userId };

    _moneyContext.Categories.Add(category);
    await _moneyContext.SaveChangesAsync(cancellationToken);

    var response = new CategoryResponse
    {
      Id = category.Id,
      Name = category.Name,
      TransactionType = category.TransacionType,
    };

    return response;
  }

  public async Task<CategoryResponse> UpdateCategoryAsync(
    string userId,
    UpdateCategoryRequest request,
    CancellationToken cancellationToken
  )
  {
    var category =
      await _moneyContext.Categories.FirstOrDefaultAsync(
        category => category.Id == request.Id && category.AppUserId == userId,
        cancellationToken
      ) ?? throw new NotFoundException("Category not found");

    category.Name = request.Name;
    category.TransacionType = request.TransactionType;

    await _moneyContext.SaveChangesAsync(cancellationToken);

    var response = new CategoryResponse
    {
      Id = category.Id,
      Name = category.Name,
      TransactionType = category.TransacionType,
    };

    return response;
  }

  public async Task<bool> DeleteCategoryAsync(
    string userId,
    int categoryId,
    CancellationToken cancellationToken
  )
  {
    var category = await _moneyContext.Categories.FirstOrDefaultAsync(
      category => category.Id == categoryId && category.AppUserId == userId,
      cancellationToken
    );

    if (category is null)
    {
      return false;
    }

    _moneyContext.Remove(category);
    await _moneyContext.SaveChangesAsync(cancellationToken);

    return true;
  }
}
