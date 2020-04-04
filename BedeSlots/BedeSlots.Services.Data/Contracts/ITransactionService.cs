using BedeSlots.Data.Models;
using BedeSlots.DTO.TransactionDto;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data.Contracts
{
    public interface ITransactionService
    {
        IQueryable<TransactionManageDto> GetAllTransactions();

        IQueryable<TransactionHistoryDto> GetUserTransactionsAsync(string id);

        Task<Transaction> AddTransactionAsync(TransactionType type, string userId, string description, decimal amount, Currency currrency);
    }
}