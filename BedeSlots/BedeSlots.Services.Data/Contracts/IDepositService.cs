using System.Threading.Tasks;
using BedeSlots.Data.Models;

namespace BedeSlots.Services.Data.Contracts
{
    public interface IDepositService
    {
        Task<Transaction> DepositAsync(Transaction transaction);
    }
}