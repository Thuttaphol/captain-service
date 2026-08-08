using System.ComponentModel.DataAnnotations;
using Captain.Models;

namespace Captain.DTOs;

public record UpdateTransactionRequest
{
  [Required]
  public int Id { get; set; }

  [MinLength(3), MaxLength(20)]
  public string Title { get; set; } = string.Empty;

  [MaxLength(100)]
  public string Description { get; set; } = string.Empty;

  [Range(0.01, 10_000_000)]
  public decimal Amount { get; set; }

  public int CategoryId { get; set; }
}
