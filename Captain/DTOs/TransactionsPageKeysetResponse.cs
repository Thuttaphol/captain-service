namespace Captain.DTOs;

public record TransactionsPageKeysetResponse<T>
{
  public required int Reference { get; init; }
  public required bool HasMore { get; init; }
  public required List<T> Data { get; init; } = [];
}
