using System.ComponentModel.DataAnnotations;
using Captain.Models;

namespace Captain.DTOs;

public record UpdateCategoryRequest
{
  [Required]
  public int Id { get; set; }

  [Required]
  [Length(3, 50, ErrorMessage = "The field Name must has length from 3 to 50.")]
  public string Name { get; set; } = string.Empty;

  [Required]
  [EnumDataType(
    typeof(TransactionType),
    ErrorMessage = "The TransactionType field must be Expense or Income."
  )]
  public TransactionType TransactionType { get; set; }
}
