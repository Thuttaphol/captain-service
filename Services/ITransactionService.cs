using Captain.DTOs;

namespace Captain.Services;

public interface ITransactionService
{
  Task<List<TransactionResponse>> GetTransactionsAsync(CancellationToken cancellationToken);
  Task<TransactionResponse?> GetTransactionByIdAsync(
    int transactionId,
    CancellationToken cancellationToken
  );
  Task<TransactionResponse> CreateTransactionAsync(
    CreateTransactionRequest request,
    CancellationToken cancellationToken
  );
  Task<TransactionResponse> UpdateTransactionAsync(
    UpdateTransactionRequest request,
    CancellationToken cancellationToken
  );
  Task<string> DeleteTransactionAsync(int transactionId, CancellationToken cancellationToken);
}
