using Captain.Models;

namespace Captain.DTOs;

public record TransactionResponse
{
  public int Id { get; init; }
  public string Title { get; init; } = string.Empty;
  public string Description { get; init; } = string.Empty;
  public decimal Amount { get; init; }
  public TransactionType TransactionType { get; init; }
  public int CategoryId { get; init; }
}
