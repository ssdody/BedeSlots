using System.Collections.Generic;
using System.Threading.Tasks;
using BedeSlots.Data.Models;

namespace BedeSlots.Services.Data.Contracts
{
    public interface ICurrencyService
    {
        ICollection<Currency> GetAllCurrencies();

        Task<Currency> GetUserCurrencyAsync(string userId);
    }
}