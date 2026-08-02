using System.ComponentModel.DataAnnotations;
using Captain.Models;

namespace Captain.DTOs;

public record UpdateCategoryRequest
{
  [Required]
  public int Id { get; set; }

  [Required]
  [MinLength(3), MaxLength(30)]
  public string Name { get; set; } = string.Empty;
}
