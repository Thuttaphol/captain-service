namespace Captain.Contracts.Errors;

public sealed record ApiValidationErrorResponse
{
  public required string Title { get; init; }
  public required int Status { get; init; }
  public required string Detail { get; init; }
  public required Dictionary<string, string[]> Errors { get; init; }
}
