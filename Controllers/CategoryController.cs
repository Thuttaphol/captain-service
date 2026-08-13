using Captain.DTOs;
using Captain.Exceptions;
using Captain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Captain.controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoryController(ICategoryService categoryService, UserManager<AppUser> userManager)
  : ControllerBase
{
  private readonly ICategoryService _categoryService = categoryService;
  private readonly UserManager<AppUser> _userManager = userManager;
  private string UserId => _userManager.GetUserId(User)!;

  [HttpGet("get-all")]
  public async Task<ActionResult<CategoryResponse>> GetCategoriesAsync(
    CancellationToken cancellationToken
  )
  {
    if (UserId is null)
    {
      return Unauthorized();
    }

    var categories = await _categoryService.GetCategoriesAsync(
      cancellationToken: cancellationToken,
      userId: UserId
    );

    return Ok(categories);
  }

  [HttpGet("{categoryId:int}")]
  public async Task<ActionResult<CategoryResponse>> GetCategoryByIdAsync(
    int categoryId,
    CancellationToken cancellationToken
  )
  {
    if (UserId is null)
    {
      throw new ForbiddenException();
    }

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
    if (UserId is null)
    {
      throw new ForbiddenException();
    }

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
    if (UserId is null)
    {
      throw new ForbiddenException();
    }

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
    if (UserId is null)
    {
      throw new ForbiddenException();
    }

    var result = await _categoryService.DeleteCategoryAsync(
      userId: UserId,
      categoryId: categoryId,
      cancellationToken: cancellationToken
    );

    return Ok(new { isDeleted = result });
  }
}
