using System.ComponentModel.DataAnnotations;

namespace Captain.DTOs;

public record CreateTransactionRequest
{
  [Required]
  [Length(3, 20, ErrorMessage = "The field Title must has lenght from 3 to 20.")]
  public required string Title { get; init; } = string.Empty;

  [MaxLength(200, ErrorMessage = "The field Description must has max length up to 200.")]
  public string Description { get; init; } = string.Empty;

  [Required]
  [Range(
    0.01,
    10_000_000,
    ErrorMessage = "The field Amount must be between 0.01 to 10,000,000"
  )]
  public decimal Amount { get; init; }

  [Required]
  public required int CategoryId { get; init; }

  [Required]
  [DataType(DataType.DateTime)]
  public required DateTimeOffset TransactionDate { get; init; }
}
