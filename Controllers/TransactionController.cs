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
  public async Task<ActionResult<TransactionResponse>> GetAllTransaction(
    CancellationToken cancellationToken
  )
  {
    var transactions = await _transactionService.GetAllTransaction(
      cancellationToken: cancellationToken
    );
    return Ok(transactions);
  }

  [HttpGet("{transactionId:int}")]
  public async Task<ActionResult<TransactionResponse>> GetTransactionById(
    int transactionId,
    CancellationToken cancellationToken
  )
  {
    var transaction = await _transactionService.GetTransactionById(
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
  public async Task<ActionResult<TransactionResponse>> CreateTransaction(
    CreateTransactionRequest request,
    CancellationToken cancellationToken
  )
  {
    var createdTransaction = await _transactionService.CreateTransaction(
      request: request,
      cancellationToken: cancellationToken
    );

    return createdTransaction;
  }

  [HttpPut]
  public async Task<ActionResult<TransactionResponse>> UpdateTransaction(
    UpdateTransactionRequest request,
    CancellationToken cancellationToken
  )
  {
    var updatedTransaction = await _transactionService.UpdateTransaction(
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
  public async Task<IActionResult> DeleteTransaction(
    [FromRoute] int transactionId,
    CancellationToken cancellationToken
  )
  {
    var response = await _transactionService.DeleteTransaction(
      transactionId: transactionId,
      cancellationToken: cancellationToken
    );

    return Ok(response);
  }
}
