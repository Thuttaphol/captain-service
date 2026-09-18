using System.Security.Claims;
using Captain.DTOs;
using Captain.Exceptions;
using Captain.Models;
using Captain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Captain.controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoriesController(ICategoryService categoryService) : ControllerBase
{
  private readonly ICategoryService _categoryService = categoryService;
  private string UserId =>
    User.FindFirstValue(ClaimTypes.NameIdentifier)
    ?? throw new InvalidOperationException("Authenticated user does not contain a user ID claim.");

  [HttpGet]
  public async Task<ActionResult<CategoryResponse>> GetCategoriesAsync(
    [FromQuery] CategorySearchQuery categorySearchQuery,
    CancellationToken cancellationToken
  )
  {
    TransactionType? transactionType = null;

    if (!string.IsNullOrWhiteSpace(categorySearchQuery.TransactionType))
    {
      var transactionTypeName = Enum.GetNames<TransactionType>()
        .FirstOrDefault(name =>
          string.Equals(
            name,
            categorySearchQuery.TransactionType,
            StringComparison.OrdinalIgnoreCase
          )
        );

      if (transactionTypeName is null)
      {
        ModelState.AddModelError(
          nameof(categorySearchQuery.TransactionType),
          "The TransactionType field must be Income or Expense."
        );

        return ValidationProblem(ModelState);
      }

      transactionType = Enum.Parse<TransactionType>(transactionTypeName);
    }
    var result = await _categoryService.GetCategoriesAsync(
      userId: UserId,
      categoryName: categorySearchQuery.Name,
      transactionType: transactionType,
      cancellationToken: cancellationToken
    );

    return Ok(result);
  }

  [HttpGet("{categoryId:int}")]
  public async Task<ActionResult<CategoryResponse>> GetCategoryByIdAsync(
    [FromRoute] int categoryId,
    CancellationToken cancellationToken
  )
  {
    var category = await _categoryService.GetCategoryByIdAsync(
      userId: UserId,
      categoryId: categoryId,
      cancellationToken: cancellationToken
    );

    if (category is null)
    {
      throw new NotFoundException("No category found");
    }

    return Ok(category);
  }

  [HttpPost]
  public async Task<ActionResult<CategoryResponse>> CreateCategoryAsync(
    CreateCategoryRequest request,
    CancellationToken cancellationToken
  )
  {
    var createdCategory = await _categoryService.CreateCategoryAsync(
      request: request,
      userId: UserId,
      cancellationToken: cancellationToken
    );

    return Ok(createdCategory);
  }

  [HttpPut]
  public async Task<ActionResult<CategoryResponse>> UpdateCategoryAsync(
    UpdateCategoryRequest request,
    CancellationToken cancellationToken
  )
  {
    var updatedCategory = await _categoryService.UpdateCategoryAsync(
      userId: UserId,
      request: request,
      cancellationToken: cancellationToken
    );

    if (updatedCategory is null)
    {
      throw new NotFoundException("No category found");
    }

    return Ok(updatedCategory);
  }

  [HttpDelete("{categoryId:int}")]
  public async Task<IActionResult> DeleteCategoryAsync(
    [FromRoute] int categoryId,
    CancellationToken cancellationToken
  )
  {
    var result = await _categoryService.DeleteCategoryAsync(
      userId: UserId,
      categoryId: categoryId,
      cancellationToken: cancellationToken
    );

    return Ok(new { isDeleted = result });
  }
}
