namespace Captain.DTOs;

public record PageResponseKeysetResponse<T>
{
  public int Reference { get; init; }
  public List<T> Data { get; init; } = [];
}
