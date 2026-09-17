using Captain.DTOs;

namespace Captain.Services;

public interface ITransactionService
{
  Task<PageResponseKeysetResponse<TransactionResponse>> GetTransactionsWithKeysetAsync(
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
}
