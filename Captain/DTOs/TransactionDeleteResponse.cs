namespace Captain.DTOs;

public record TransactionDeleteResponse
{
  public required bool IsDeleted { get; init; }
}
