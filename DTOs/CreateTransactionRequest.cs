using System.ComponentModel.DataAnnotations;
using Captain.Models;

namespace Captain.DTOs;

public record CreateTransactionRequest
{
  [Required]
  [MinLength(3), MaxLength(20)]
  public string Title { get; set; } = string.Empty;

  [MaxLength(200)]
  public string Description { get; set; } = string.Empty;

  [Required]
  [Range(0.01, 10_000_000)]
  public decimal Amount { get; set; }

  [Required]
  [EnumDataType(typeof(TransactionType))]
  public TransactionType TransactionType { get; set; }

  [Required]
  public int CategoryId { get; set; }
}
