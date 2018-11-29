using BedeSlots.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data.Contracts
{
    public interface ITransactionService
    {
        Task<Transaction> RegisterTransactionsAsync(Transaction transaction);

        Task<ICollection<Transaction>> GetAllTransactionsAsync();

        Task<Transaction> GetTransactionByIdAsync(int id);

        Transaction CreateTransaction(TransactionType type, string userId, int cardId, decimal depositAmount);
    }
}