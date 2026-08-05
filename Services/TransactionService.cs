using Captain.Data;
using Captain.DTOs;
using Captain.Models;
using Microsoft.EntityFrameworkCore;

namespace Captain.Services;

public class TransactionService(MoneyDbContext moneyContext) : ITransactionService
{
  private readonly MoneyDbContext _moneyContext = moneyContext;

  public async Task<List<TransactionResponse>> GetTransactionsAsync(
    string userId,
    CancellationToken cancellationToken
  )
  {
    List<TransactionResponse> response = await _moneyContext
      .Transactions.AsNoTracking()
      .Where(transaction => transaction.AppUserId == userId)
      .OrderByDescending(transaction => transaction.UpdatedDate)
      .Select(transaction => new TransactionResponse
      {
        Id = transaction.Id,
        Title = transaction.Title,
        Description = transaction.Description,
        Amount = transaction.Amount,
        TransactionType = transaction.TransactionType,
        CategoryId = transaction.CategoryId,
      })
      .ToListAsync(cancellationToken);

    return response;
  }

  public async Task<TransactionResponse?> GetTransactionByIdAsync(
    string userId,
    int transactionId,
    CancellationToken cancellationToken
  )
  {
    TransactionResponse? response = await _moneyContext
      .Transactions.AsNoTracking()
      .Where(transaction => transaction.Id == transactionId && transaction.AppUserId == userId)
      .Select(transaction => new TransactionResponse
      {
        Id = transaction.Id,
        Title = transaction.Title,
        Description = transaction.Description,
        Amount = transaction.Amount,
        TransactionType = transaction.TransactionType,
        CategoryId = transaction.CategoryId,
      })
      .FirstOrDefaultAsync(cancellationToken);

    return response;
  }

  public async Task<TransactionResponse> CreateTransactionAsync(
    string userId,
    CreateTransactionRequest request,
    CancellationToken cancellationToken
  )
  {
    bool isCategoryExist = await _moneyContext.Categories.AnyAsync(
      category => category.Id == request.CategoryId,
      cancellationToken
    );

    if (!isCategoryExist)
    {
      throw new ArgumentException($"Category with ID {request.CategoryId} does not exist.");
    }

    var transaction = new Transaction
    {
      Title = request.Title,
      Description = request.Description,
      Amount = request.Amount,
      TransactionType = request.TransactionType,
      CategoryId = request.CategoryId,
      UpdatedDate = DateTime.UtcNow,
      // Category is required by the C# property, but EF will associate
      // it using CategoryId. We do not need to load the full Category
      // before inserting.
      Category = null!,
      AppUserId = userId,
    };

    _moneyContext.Transactions.Add(transaction);
    await _moneyContext.SaveChangesAsync(cancellationToken);

    var response = new TransactionResponse
    {
      Id = transaction.Id,
      Title = transaction.Title,
      Description = transaction.Description,
      Amount = transaction.Amount,
      TransactionType = transaction.TransactionType,
      CategoryId = transaction.CategoryId,
    };

    return response;
  }

  public async Task<TransactionResponse> UpdateTransactionAsync(
    string userId,
    UpdateTransactionRequest request,
    CancellationToken cancellationToken
  )
  {
    bool isCategoryExist = await _moneyContext.Categories.AnyAsync(
      category => category.Id == request.CategoryId,
      cancellationToken
    );

    if (!isCategoryExist)
    {
      throw new ArgumentException($"Category with ID {request.CategoryId} does not exist.");
    }

    var transaction =
      await _moneyContext.Transactions.FirstOrDefaultAsync(
        transaction => transaction.Id == request.Id,
        cancellationToken
      ) ?? throw new Exception("Transaction not found");

    transaction.Title = request.Title;
    transaction.Description = request.Description;
    transaction.Amount = request.Amount;
    transaction.TransactionType = request.TransactionType;
    transaction.CategoryId = request.CategoryId;
    transaction.UpdatedDate = DateTime.UtcNow;

    await _moneyContext.SaveChangesAsync(cancellationToken);

    var response = new TransactionResponse
    {
      Id = transaction.Id,
      Title = transaction.Title,
      Description = transaction.Description,
      Amount = transaction.Amount,
      TransactionType = transaction.TransactionType,
      CategoryId = transaction.CategoryId,
    };

    return response;
  }

  public async Task<string> DeleteTransactionAsync(
    string userId,
    int transactionId,
    CancellationToken cancellationToken
  )
  {
    var transaction = await _moneyContext.Transactions.FirstOrDefaultAsync(
      transaction => transaction.Id == transactionId,
      cancellationToken
    );

    if (transaction is null)
    {
      return "Transaction not found.";
    }

    _moneyContext.Transactions.Remove(transaction);

    await _moneyContext.SaveChangesAsync(cancellationToken);

    return "Transaction deleted.";
  }
}
