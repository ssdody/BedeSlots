using System.Collections.Generic;
using System.Threading.Tasks;
using BedeSlots.Data.Models;

namespace BedeSlots.Services.Data.Contracts
{
    public interface IExchangeRateApiService
    {
        Task<IDictionary<Currency, decimal>> GetAllRatesAsync();
        Task<decimal> GetRateAsync(Currency currencyName);
    }
}