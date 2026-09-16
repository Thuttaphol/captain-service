namespace Captain.DTOs;

public record TransactionSearchQuery
{
  public string? Title { get; init; }
  public string? Description { get; init; }
  public int? CategoryId { get; init; }
}
