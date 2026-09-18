using Captain.Models;

namespace Captain.DTOs;

public record TransactionResponse
{
  public required int Id { get; init; }
  public required string Title { get; init; }
  public required string Description { get; init; }
  public required decimal Amount { get; init; }
  public required int CategoryId { get; init; }
}
