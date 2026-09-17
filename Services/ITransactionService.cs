using Captain.DTOs;
using Captain.Models;

namespace Captain.Services;

public interface ITransactionService
{
  Task<List<TransactionResponse>> GetTransactionsAsync(
    string userId,
    TransactionSearchQuery transactionSearchQuery,
    CancellationToken cancellationToken
  );
  Task<TransactionResponse?> GetTransactionByIdAsync(
    string userId,
    int transactionId,
    CancellationToken cancellationToken
  );
  Task<TransactionResponse> CreateTransactionAsync(
    string userId,
    CreateTransactionRequest request,
    CancellationToken cancellationToken
  );
  Task<TransactionResponse> UpdateTransactionAsync(
    string userId,
    UpdateTransactionRequest request,
    CancellationToken cancellationToken
  );
  Task<bool> DeleteTransactionAsync(
    string userId,
    int transactionId,
    CancellationToken cancellationToken
  );

  Task<PageResponseKeysetResponse<TransactionResponse>> GetWithKeysetPagination(
    int reference,
    int pageSize,
    string userId,
    CancellationToken cancellationToken
  );
}
