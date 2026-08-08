using Captain.Models;

namespace Captain.DTOs;

public record CategoryResponse
{
  public required int Id { get; init; }
  public required string Name { get; init; } = string.Empty;
  public required TransactionType TransactionType { get; init; }
}
