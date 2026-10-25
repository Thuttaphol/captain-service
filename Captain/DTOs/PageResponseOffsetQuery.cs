using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Captain.DTOs;

public record PageResponseOffsetQuery
{
  [BindRequired]
  public int PageNumber { get; init; }

  [BindRequired]
  public int PageSize { get; init; }
}
