using Captain.DTOs;
using Captain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Captain.controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController(ITransactionService transactionService) : ControllerBase
{
  private readonly ITransactionService _transactionService = transactionService;

  [HttpGet("get-all")]
  public async Task<ActionResult<TransactionResponse>> GetTransactionsAsync(
    CancellationToken cancellationToken
  )
  {
    var transactions = await _transactionService.GetTransactionsAsync(
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
    var transaction = await _transactionService.GetTransactionByIdAsync(
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
    var createdTransaction = await _transactionService.CreateTransactionAsync(
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
    var updatedTransaction = await _transactionService.UpdateTransactionAsync(
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
    var response = await _transactionService.DeleteTransactionAsync(
      transactionId: transactionId,
      cancellationToken: cancellationToken
    );

    return Ok(response);
  }
}
