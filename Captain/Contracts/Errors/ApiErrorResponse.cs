namespace Captain.Contracts.Errors;

public sealed record ApiErrorResponse
{
  public required string Title { get; init; }
  public required int Status { get; init; }
  public required string Detail { get; init; }
  public string? TraceId { get; init; }
}
