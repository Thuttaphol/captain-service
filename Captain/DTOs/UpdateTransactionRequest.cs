using System.ComponentModel.DataAnnotations;
using Captain.Models;

namespace Captain.DTOs;

public record UpdateTransactionRequest
{
  [Required]
  public required int Id { get; init; }

  [Length(3, 20, ErrorMessage = "The field Title must has lenght from 3 to 20.")]
  public string Title { get; init; } = string.Empty;

  [MaxLength(200, ErrorMessage = "The field Description must has max length up to 200.")]
  public string Description { get; init; } = string.Empty;

  [Range(
    0.01,
    10_000_000,
    ErrorMessage = "The field Amount must be between 0.01 to 10,000,000"
  )]
  public decimal Amount { get; init; }
  public int CategoryId { get; init; }

  [DataType(DataType.DateTime)]
  public DateTimeOffset TransactionDate { get; init; }
}
