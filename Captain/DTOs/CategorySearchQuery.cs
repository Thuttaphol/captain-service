using System.ComponentModel.DataAnnotations;
using Captain.Models;

namespace Captain.DTOs;

public record CategorySearchQuery
{
  public string? Name { get; init; }

  [EnumDataType(
    typeof(TransactionType),
    ErrorMessage = "The TransactionType field must be Expense or Income."
  )]
  public string? TransactionType { get; init; }
}
