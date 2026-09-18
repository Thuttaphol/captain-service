using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Captain.DTOs;

public record TransactionSearchQuery
{
  public string? Title { get; init; }
  public string? Description { get; init; }
  public int? CategoryId { get; init; }

  //paging
  [BindRequired]
  public int Reference { get; init; }

  [BindRequired]
  public int PageSize { get; init; }
}
