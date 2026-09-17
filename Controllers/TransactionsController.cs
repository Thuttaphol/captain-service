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

    var transactions = await _transactionService.GetTransactionsAsync(
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

  [HttpGet("paging")]
  public async Task<
    ActionResult<PageResponseKeysetResponse<TransactionResponse>>
  > GetWithKeysetPagination(
    [FromQuery] TransactionPageKeysetRequest request,
    CancellationToken cancellationToken
  )
  {
    if (UserId is null)
    {
      throw new ForbiddenException();
    }

    var response = await _transactionService.GetWithKeysetPagination(
      reference: request.Reference,
      pageSize: request.PageSize,
      userId: UserId,
      cancellationToken: cancellationToken
    );

    return Ok(response);
  }
}
