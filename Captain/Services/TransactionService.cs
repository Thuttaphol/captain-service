using Captain.Data;
using Captain.DTOs;
using Captain.Models;
using Microsoft.EntityFrameworkCore;

namespace Captain.Services;

public class TransactionService(MoneyDbContext moneyContext) : ITransactionService
{
  private readonly MoneyDbContext _moneyContext = moneyContext;

  public async Task<
    TransactionsPageKeysetResponse<TransactionResponse>
  > GetTransactionsWithKeysetAsync(
    string userId,
    TransactionSearchQuery transactionSearchQuery,
    CancellationToken cancellationToken
  )
  {
    var query = _moneyContext
      .Transactions.AsNoTracking()
      .Where(transaction => transaction.AppUserId == userId);

    if (!string.IsNullOrWhiteSpace(transactionSearchQuery.Title))
    {
      query = query.Where(transaction =>
        EF.Functions.ILike(transaction.Title, $"%{transactionSearchQuery.Title.Trim()}%")
      );
    }

    if (!string.IsNullOrWhiteSpace(transactionSearchQuery.Description))
    {
      query = query.Where(transaction =>
        EF.Functions.ILike(
          transaction.Description,
          $"%{transactionSearchQuery.Description.Trim()}%"
        )
      );
    }

    if (transactionSearchQuery.CategoryId is not null)
    {
      query = query.Where(transaction =>
        transaction.CategoryId == transactionSearchQuery.CategoryId
      );
    }

    if (transactionSearchQuery.Reference is not null)
    {
      query = query.Where(transaction =>
        transaction.Id < transactionSearchQuery.Reference
      );
    }

    var transactions = await query
      .OrderByDescending(transaction => transaction.Id)
      .Take(transactionSearchQuery.PageSize)
      .Select(transaction => new TransactionResponse
      {
        Id = transaction.Id,
        Title = transaction.Title,
        Description = transaction.Description,
        Amount = transaction.Amount,
        CategoryName = transaction.Category.Name,
        TransactionDate = transaction.TransactionDate,
      })
      .ToListAsync(cancellationToken);

    var newReference = transactions.Count != 0 ? transactions.Last().Id : 0;
    var hasMore = transactions.Count >= transactionSearchQuery.PageSize;

    var response = new TransactionsPageKeysetResponse<TransactionResponse>
    {
      Reference = newReference,
      HasMore = hasMore,
      Data = transactions,
    };
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
      .Where(transaction =>
        transaction.Id == transactionId && transaction.AppUserId == userId
      )
      .Select(transaction => new TransactionResponse
      {
        Id = transaction.Id,
        Title = transaction.Title,
        Description = transaction.Description,
        Amount = transaction.Amount,
        CategoryName = transaction.Category.Name,
        TransactionDate = transaction.TransactionDate,
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
    var category = await _moneyContext.Categories.FirstOrDefaultAsync(
      category => category.Id == request.CategoryId && category.AppUserId == userId,
      cancellationToken
    );

    if (category is null)
    {
      throw new ArgumentException(
        $"Category with ID {request.CategoryId} does not exist."
      );
    }

    var transaction = new Transaction
    {
      Title = request.Title,
      Description = request.Description,
      Amount = request.Amount,
      Category = category,
      TransactionDate = request.TransactionDate.UtcDateTime,
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
      CategoryName = transaction.Category.Name,
      TransactionDate = transaction.TransactionDate,
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
      throw new ArgumentException(
        $"Category with ID {request.CategoryId} does not exist."
      );
    }

    var transaction =
      await _moneyContext.Transactions.FirstOrDefaultAsync(
        transaction => transaction.Id == request.Id,
        cancellationToken
      ) ?? throw new Exception("Transaction not found");

    transaction.Title = request.Title;
    transaction.Description = request.Description;
    transaction.Amount = request.Amount;
    transaction.CategoryId = request.CategoryId;
    transaction.TransactionDate = request.TransactionDate.UtcDateTime;

    await _moneyContext.SaveChangesAsync(cancellationToken);

    var response = new TransactionResponse
    {
      Id = transaction.Id,
      Title = transaction.Title,
      Description = transaction.Description,
      Amount = transaction.Amount,
      CategoryName = transaction.Category.Name,
      TransactionDate = transaction.TransactionDate,
    };

    return response;
  }

  public async Task<bool> DeleteTransactionAsync(
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
      return false;
    }

    _moneyContext.Transactions.Remove(transaction);

    await _moneyContext.SaveChangesAsync(cancellationToken);

    return true;
  }

  public async Task<
    PageResponseOffsetResponse<TransactionResponse>
  > GetTransactionsWithOffsetAsync(
    string userId,
    PageResponseOffsetQuery pageResponseOffsetQuery,
    CancellationToken cancellationToken
  )
  {
    var totalRecords = await _moneyContext
      .Transactions.AsNoTracking()
      .CountAsync(cancellationToken);

    var transactions = await _moneyContext
      .Transactions.AsNoTracking()
      .OrderBy(transaction => transaction.Id)
      .Skip((pageResponseOffsetQuery.PageNumber - 1) * pageResponseOffsetQuery.PageSize)
      .Take(pageResponseOffsetQuery.PageSize)
      .Select(transaction => new TransactionResponse
      {
        Id = transaction.Id,
        Title = transaction.Title,
        Description = transaction.Description,
        Amount = transaction.Amount,
        CategoryName = transaction.Category.Name,
        TransactionDate = transaction.TransactionDate,
      })
      .ToListAsync(cancellationToken);

    var response = new PageResponseOffsetResponse<TransactionResponse>
    {
      PageNumber = pageResponseOffsetQuery.PageNumber,
      PageSize = pageResponseOffsetQuery.PageSize,
      TotalRecords = totalRecords,
      TotalPages = (int)
        Math.Ceiling((decimal)totalRecords / (decimal)pageResponseOffsetQuery.PageSize),
      Data = transactions,
    };

    return response;
  }

  public async Task<TotalBalanceResponse> CalculateTotalBalance(
    string userId,
    CancellationToken cancellationToken
  )
  {
    var transactions = await _moneyContext
      .Transactions.AsNoTracking()
      .Where(transaction => transaction.AppUserId == userId)
      .Select(transaction => new
      {
        transaction.Category.TransactionType,
        transaction.Amount,
      })
      .ToListAsync(cancellationToken);

    var incomeTransactions = transactions
      .Where(t => t.TransactionType == TransactionType.Income)
      .Select(t => t.Amount);
    var expenseTransactions = transactions
      .Where(t => t.TransactionType == TransactionType.Expense)
      .Select(t => t.Amount);

    var totalIncome = incomeTransactions.Sum();
    var totalExpense = expenseTransactions.Sum();

    var totalBalance = totalIncome - totalExpense;

    return new TotalBalanceResponse { TotalBalance = totalBalance };
  }
}
