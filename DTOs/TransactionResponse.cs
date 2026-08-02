using Captain.Models;

namespace Captain.DTOs;

public record TransactionResponse
{
  public int Id;
  public string Title = string.Empty;
  public string Description = string.Empty;
  public decimal Amount;
  public TransactionType TransactionType;
  public int CategoryId;
}
