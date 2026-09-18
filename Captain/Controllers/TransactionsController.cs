using Captain.DTOs;
using Captain.Exceptions;
using Captain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Captain.controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TransactionsController(
  ITransactionService transactionService,
  UserManager<AppUser> userManager
) : ControllerBase
{
  private readonly ITransactionService _transactionService = transactionService;
  private readonly UserManager<AppUser> _userManager = userManager;
  private string UserId => _userManager.GetUserId(User)!;

  [HttpGet]
  public async Task<ActionResult<TransactionResponse>> GetTransactionsAsync(
    [FromQuery] TransactionSearchQuery transactionSearchQuery,
    CancellationToken cancellationToken
  )
  {
    if (UserId is null)
    {
      throw new ForbiddenException();
    }

    if (transactionSearchQuery.Reference < 0 && transactionSearchQuery.PageSize <= 0)
    {
      ModelState.AddModelError(
        nameof(transactionSearchQuery.Reference),
        "The Reference field lowest value is 0."
      );
      ModelState.AddModelError(
        nameof(transactionSearchQuery.PageSize),
        "The Pagesize must greater than 0."
      );

      return ValidationProblem(ModelState);
    }
    else if (transactionSearchQuery.Reference < 0)
    {
      ModelState.AddModelError(
        nameof(transactionSearchQuery.Reference),
        "The Reference field lowest value is 0."
      );

      return ValidationProblem(ModelState);
    }
    else if (transactionSearchQuery.PageSize <= 0)
    {
      ModelState.AddModelError(
        nameof(transactionSearchQuery.PageSize),
        "The Pagesize must greater than 0."
      );

      return ValidationProblem(ModelState);
    }

    var transactions = await _transactionService.GetTransactionsWithKeysetAsync(
      userId: UserId,
      transactionSearchQuery: transactionSearchQuery,
      cancellationToken: cancellationToken
    );
    return Ok(transactions);
  }

  [HttpGet("{transactionId:int}")]
  public async Task<ActionResult<TransactionResponse>> GetTransactionByIdAsync(
    int transactionId,
    CancellationToken cancellationToken
  )
  {
    if (UserId is null)
    {
      throw new ForbiddenException();
    }

    var transaction = await _transactionService.GetTransactionByIdAsync(
      userId: UserId,
      transactionId,
      cancellationToken
    );

    if (transaction is null)
    {
      throw new NotFoundException("No transaction found");
    }

    return Ok(transaction);
  }

  [HttpPost]
  public async Task<ActionResult<TransactionResponse>> CreateTransactionAsync(
    CreateTransactionRequest request,
    CancellationToken cancellationToken
  )
  {
    if (UserId is null)
    {
      throw new ForbiddenException();
    }

    var createdTransaction = await _transactionService.CreateTransactionAsync(
      userId: UserId,
      request: request,
      cancellationToken: cancellationToken
    );

    return createdTransaction;
  }

  [HttpPut]
  public async Task<ActionResult<TransactionResponse>> UpdateTransactionAsync(
    UpdateTransactionRequest request,
    CancellationToken cancellationToken
  )
  {
    if (UserId is null)
    {
      throw new ForbiddenException();
    }

    var updatedTransaction = await _transactionService.UpdateTransactionAsync(
      userId: UserId,
      request: request,
      cancellationToken: cancellationToken
    );

    if (updatedTransaction is null)
    {
      throw new NotFoundException("No transaction found");
    }
    return updatedTransaction;
  }

  [HttpDelete("{transactionId:int}")]
  public async Task<IActionResult> DeleteTransactionAsync(
    [FromRoute] int transactionId,
    CancellationToken cancellationToken
  )
  {
    if (UserId is null)
    {
      throw new ForbiddenException();
    }

    var result = await _transactionService.DeleteTransactionAsync(
      userId: UserId,
      transactionId: transactionId,
      cancellationToken: cancellationToken
    );

    return Ok(new { isDeleted = result });
  }

  [HttpGet("offset")]
  public async Task<
    ActionResult<PageResponseOffsetResponse<TransactionResponse>>
  > GetTransactionsOffsetAsync(
    [FromQuery] PageResponseOffsetQuery pageResponseOffsetQuery,
    CancellationToken cancellationToken
  )
  {
    if (UserId is null)
    {
      throw new ForbiddenException();
    }

    if (pageResponseOffsetQuery.PageNumber <= 0 && pageResponseOffsetQuery.PageSize <= 0)
    {
      ModelState.AddModelError(
        nameof(pageResponseOffsetQuery.PageNumber),
        "The PageNumber field must greater than 0."
      );
      ModelState.AddModelError(
        nameof(pageResponseOffsetQuery.PageSize),
        "The PageSize field must greater than 0."
      );

      return ValidationProblem(ModelState);
    }
    else if (pageResponseOffsetQuery.PageNumber <= 0)
    {
      ModelState.AddModelError(
        nameof(pageResponseOffsetQuery.PageNumber),
        "The PageNumber field must greater than 0."
      );

      return ValidationProblem(ModelState);
    }
    else if (pageResponseOffsetQuery.PageSize <= 0)
    {
      ModelState.AddModelError(
        nameof(pageResponseOffsetQuery.PageSize),
        "The PageSize field must greater than 0."
      );

      return ValidationProblem(ModelState);
    }

    var transactions = await _transactionService.GetTransactionsWithOffsetAsync(
      userId: UserId,
      pageResponseOffsetQuery: pageResponseOffsetQuery,
      cancellationToken: cancellationToken
    );

    return Ok(transactions);
  }

  [HttpGet("total-balance")]
  public async Task<ActionResult<TotalBalanceResponse>> TotalBalance(
    CancellationToken cancellationToken
  )
  {
    var totalBalance = await _transactionService.CalculateTotalBalance(
      userId: UserId,
      cancellationToken: cancellationToken
    );

    return Ok(totalBalance);
  }
}
