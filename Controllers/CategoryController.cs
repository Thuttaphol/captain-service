using Captain.DTOs;
using Captain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Captain.controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
  private readonly ICategoryService _categoryService = categoryService;

  [HttpGet("get-all")]
  public async Task<ActionResult<CategoryResponse>> GetCategoriesAsync(
    CancellationToken cancellationToken
  )
  {
    var categories = await _categoryService.GetCategoriesAsync(
      cancellationToken: cancellationToken
    );

    return Ok(categories);
  }

  [HttpGet("{categoryId:int}")]
  public async Task<ActionResult<CategoryResponse>> GetCategoryByIdAsync(
    int categoryId,
    CancellationToken cancellationToken
  )
  {
    var category = await _categoryService.GetCategoryByIdAsync(
      CategoryId: categoryId,
      cancellationToken: cancellationToken
    );

    if (category is null)
    {
      return NotFound();
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
      request: request,
      cancellationToken: cancellationToken
    );

    if (updatedCategory is null)
    {
      return NotFound();
    }

    return Ok(updatedCategory);
  }

  [HttpDelete("{categoryId:int}")]
  public async Task<IActionResult> DeleteCategoryAsync(
    [FromRoute] int categoryId,
    CancellationToken cancellationToken
  )
  {
    var response = await _categoryService.DeleteCategoryAsync(
      categoryId: categoryId,
      cancellationToken: cancellationToken
    );

    return Ok(response);
  }
}
