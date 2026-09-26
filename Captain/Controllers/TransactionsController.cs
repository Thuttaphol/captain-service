using System.Security.Claims;
using Captain.Contracts.Errors;
using Captain.DTOs;
using Captain.Exceptions;
using Captain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Captain.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TransactionsController(ITransactionService transactionService) : ControllerBase
{
  private readonly ITransactionService _transactionService = transactionService;
  private string UserId =>
    User.FindFirstValue(ClaimTypes.NameIdentifier)
    ?? throw new InvalidOperationException("Authenticated user does not contain a user ID claim.");

  [HttpGet]
  [ProducesResponseType<List<TransactionResponse>>(
    StatusCodes.Status200OK,
    "application/json",
    Description = "Transactions return successfully"
  )]
  [ProducesResponseType<ApiValidationErrorResponse>(
    StatusCodes.Status400BadRequest,
    "application/json",
    Description = "Invalid paratmeters"
  )]
  public async Task<ActionResult<List<TransactionResponse>>> GetTransactionsAsync(
    [FromQuery] TransactionSearchQuery transactionSearchQuery,
    CancellationToken cancellationToken
  )
  {
    if (transactionSearchQuery.Reference < 0)
    {
      ModelState.AddModelError(
        nameof(transactionSearchQuery.Reference),
        "The Reference field lowest value is 0."
      );

      return ValidationProblem(ModelState);
    }
    if (transactionSearchQuery.PageSize <= 0)
    {
      ModelState.AddModelError(
        nameof(transactionSearchQuery.PageSize),
        "The Pagesize must greater than 0."
      );

      return ValidationProblem(ModelState);
    }

    if (!ModelState.IsValid)
    {
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
  [ProducesResponseType<TransactionResponse>(
    StatusCodes.Status200OK,
    "application/json",
    Description = "A transaction return successfully"
  )]
  [ProducesResponseType<ApiErrorResponse>(
    StatusCodes.Status404NotFound,
    "application/json",
    Description = "Transaction not found"
  )]
  public async Task<ActionResult<TransactionResponse>> GetTransactionByIdAsync(
    int transactionId,
    CancellationToken cancellationToken
  )
  {
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
  [ProducesResponseType<TransactionResponse>(
    StatusCodes.Status200OK,
    "application/json",
    Description = "Create transaction return successfully"
  )]
  [ProducesResponseType<ApiValidationErrorResponse>(
    StatusCodes.Status400BadRequest,
    "application/json",
    Description = "Invalid paratmeters"
  )]
  public async Task<ActionResult<TransactionResponse>> CreateTransactionAsync(
    CreateTransactionRequest request,
    CancellationToken cancellationToken
  )
  {
    var createdTransaction = await _transactionService.CreateTransactionAsync(
      userId: UserId,
      request: request,
      cancellationToken: cancellationToken
    );

    return createdTransaction;
  }

  [HttpPut]
  [ProducesResponseType<TransactionResponse>(
    StatusCodes.Status200OK,
    "application/json",
    Description = "Update transaction return successfully"
  )]
  [ProducesResponseType<ApiValidationErrorResponse>(
    StatusCodes.Status400BadRequest,
    "application/json",
    Description = "Invalid paratmeters"
  )]
  [ProducesResponseType<ApiErrorResponse>(
    StatusCodes.Status404NotFound,
    "application/json",
    Description = "Transaction not found"
  )]
  public async Task<ActionResult<TransactionResponse>> UpdateTransactionAsync(
    UpdateTransactionRequest request,
    CancellationToken cancellationToken
  )
  {
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
  [ProducesResponseType<TransactionDeleteResponse>(
    StatusCodes.Status200OK,
    "application/json",
    Description = "Delete transaction successfully"
  )]
  [ProducesResponseType<ApiErrorResponse>(
    StatusCodes.Status404NotFound,
    "application/json",
    Description = "Transaction not found"
  )]
  public async Task<ActionResult<TransactionDeleteResponse>> DeleteTransactionAsync(
    [FromRoute] int transactionId,
    CancellationToken cancellationToken
  )
  {
    var result = await _transactionService.DeleteTransactionAsync(
      userId: UserId,
      transactionId: transactionId,
      cancellationToken: cancellationToken
    );

    return Ok(new TransactionDeleteResponse { IsDeleted = result });
  }

  [HttpGet("offset")]
  [ProducesResponseType<PageResponseOffsetResponse<TransactionResponse>>(
    StatusCodes.Status200OK,
    "application/json",
    Description = "Transactions return successfully"
  )]
  [ProducesResponseType<ApiValidationErrorResponse>(
    StatusCodes.Status400BadRequest,
    "application/json",
    Description = "Invalid paratmeters"
  )]
  public async Task<
    ActionResult<PageResponseOffsetResponse<TransactionResponse>>
  > GetTransactionsOffsetAsync(
    [FromQuery] PageResponseOffsetQuery pageResponseOffsetQuery,
    CancellationToken cancellationToken
  )
  {
    if (pageResponseOffsetQuery.PageNumber <= 0)
    {
      ModelState.AddModelError(
        nameof(pageResponseOffsetQuery.PageNumber),
        "The PageNumber field must greater than 0."
      );

      return ValidationProblem(ModelState);
    }
    if (pageResponseOffsetQuery.PageSize <= 0)
    {
      ModelState.AddModelError(
        nameof(pageResponseOffsetQuery.PageSize),
        "The PageSize field must greater than 0."
      );

      return ValidationProblem(ModelState);
    }

    if (!ModelState.IsValid)
    {
      return ValidationProblem(ModelState);
    }

    var response = await _transactionService.GetTransactionsWithOffsetAsync(
      userId: UserId,
      pageResponseOffsetQuery: pageResponseOffsetQuery,
      cancellationToken: cancellationToken
    );

    return Ok(response);
  }

  [HttpGet("total-balance")]
  [ProducesResponseType<TotalBalanceResponse>(
    StatusCodes.Status200OK,
    "application/json",
    Description = "TotalBalance return successfully"
  )]
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
