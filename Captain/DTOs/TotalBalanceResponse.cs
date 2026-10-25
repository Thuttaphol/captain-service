namespace Captain.DTOs;

public record TotalBalanceResponse
{
  public required decimal TotalBalance { get; init; }
}
