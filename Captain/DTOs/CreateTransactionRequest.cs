using System.ComponentModel.DataAnnotations;

namespace Captain.DTOs;

public record CreateTransactionRequest
{
  [Required]
  [Length(3, 20, ErrorMessage = "The field Title must has lenght from 3 to 20.")]
  public string Title { get; set; } = string.Empty;

  [MaxLength(200, ErrorMessage = "The field Description must has max length up to 200.")]
  public string Description { get; set; } = string.Empty;

  [Required]
  [Range(0.01, 10_000_000, ErrorMessage = "The field Amount must be between 0.01 to 10,000,000")]
  public decimal Amount { get; set; }

  [Required]
  public int CategoryId { get; set; }
}
