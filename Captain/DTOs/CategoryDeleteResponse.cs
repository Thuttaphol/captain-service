namespace Captain.DTOs;

public record CategoryDeleteResponse
{
  public required bool IsDeleted { get; init; }
}
