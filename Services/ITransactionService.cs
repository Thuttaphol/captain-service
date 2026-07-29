using Captain.DTOs;

namespace Captain.Services;

public interface ITransactionService
{
  Task<List<TransactionResponse>> GetAllTransaction(CancellationToken cancellationToken);
  Task<TransactionResponse?> GetTransactionById(
    int transactionId,
    CancellationToken cancellationToken
  );
  Task<TransactionResponse> CreateTransaction(
    CreateTransactionRequest request,
    CancellationToken cancellationToken
  );
  Task<TransactionResponse> UpdateTransaction(
    UpdateTransactionRequest request,
    CancellationToken cancellationToken
  );
  Task<string> DeleteTransaction(int transactionId, CancellationToken cancellationToken);
}
