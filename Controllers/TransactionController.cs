using Captain.DTOs;
using Captain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Captain.controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TransactionController(
  ITransactionService transactionService,
  UserManager<AppUser> userManager
) : ControllerBase
{
  private readonly ITransactionService _transactionService = transactionService;
  private readonly UserManager<AppUser> _userManager = userManager;
  private string UserId => _userManager.GetUserId(User)!;

  [HttpGet("get-all")]
  public async Task<ActionResult<TransactionResponse>> GetTransactionsAsync(
    CancellationToken cancellationToken
  )
  {
    if (UserId is null)
    {
      return Unauthorized();
    }

    var transactions = await _transactionService.GetTransactionsAsync(
      userId: UserId,
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
      return Unauthorized();
    }

    var transaction = await _transactionService.GetTransactionByIdAsync(
      userId: UserId,
      transactionId,
      cancellationToken
    );

    if (transaction is null)
    {
      return NotFound();
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
      return Unauthorized();
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
      return Unauthorized();
    }

    var updatedTransaction = await _transactionService.UpdateTransactionAsync(
      userId: UserId,
      request: request,
      cancellationToken: cancellationToken
    );

    if (updatedTransaction is null)
    {
      return NotFound();
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
      return Unauthorized();
    }

    var response = await _transactionService.DeleteTransactionAsync(
      userId: UserId,
      transactionId: transactionId,
      cancellationToken: cancellationToken
    );

    return Ok(response);
  }
}
