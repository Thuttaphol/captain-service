using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Captain.DTOs;

public record TransactionPageKeysetRequest
{
  [BindRequired]
  public int Reference { get; init; }

  [BindRequired]
  public int PageSize { get; init; }
}
