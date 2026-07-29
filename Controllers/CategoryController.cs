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
  public async Task<ActionResult<CategoryResponse>> GetAllCategory(
    CancellationToken cancellationToken
  )
  {
    var categories = await _categoryService.GetAllCategory(cancellationToken: cancellationToken);

    return Ok(categories);
  }

  [HttpGet("{categoryId:int}")]
  public async Task<ActionResult<CategoryResponse>> GetCategoryById(
    int categoryId,
    CancellationToken cancellationToken
  )
  {
    var category = await _categoryService.GetCategoryById(
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
  public async Task<ActionResult<CategoryResponse>> CreateCategory(
    CreateCategoryRequest request,
    CancellationToken cancellationToken
  )
  {
    var createdCategory = await _categoryService.CreateCategory(
      request: request,
      cancellationToken: cancellationToken
    );

    return Ok(createdCategory);
  }

  [HttpPut]
  public async Task<ActionResult<CategoryResponse>> UpdateCategory(
    UpdateCategoryRequest request,
    CancellationToken cancellationToken
  )
  {
    var updatedCategory = await _categoryService.UpdateCategory(
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
  public async Task<IActionResult> DeleteCategory(
    [FromRoute] int categoryId,
    CancellationToken cancellationToken
  )
  {
    var response = await _categoryService.DeleteCategory(
      categoryId: categoryId,
      cancellationToken: cancellationToken
    );

    return Ok(response);
  }
}
