using System.ComponentModel.DataAnnotations;
using Captain.Models;

namespace Captain.DTOs;

public record CreateCategoryRequest
{
  [Required]
  [MinLength(3), MaxLength(30)]
  public string Name { get; set; } = string.Empty;

  [Required]
  [EnumDataType(typeof(TransactionType))]
  public TransactionType TransactionType { get; set; }
}
