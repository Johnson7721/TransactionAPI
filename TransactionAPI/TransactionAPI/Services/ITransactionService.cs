using TransactionAPI.Models;

namespace TransactionAPI.Services
{
    public interface ITransactionService
    {
        Task<TransactionResponse> ProcessTransactionAsync(TransactionRequest request, CancellationToken cancellationToken= default);
    }
}
