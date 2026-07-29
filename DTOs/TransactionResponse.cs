using Captain.Models;

namespace Captain.DTOs;

public record TransactionResponse
{
  public required int Id;
  public required string Title = string.Empty;
  public required string Description = string.Empty;
  public required decimal Amount;
  public required TransactionType TransactionType;
  public required int CategoryId;
}
