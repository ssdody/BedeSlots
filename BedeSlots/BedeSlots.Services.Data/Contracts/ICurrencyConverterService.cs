using System.Threading.Tasks;
using BedeSlots.Data.Models;

namespace BedeSlots.Services.Data.Contracts
{
    public interface ICurrencyConverterService
    {
        Task<decimal> ConvertToBaseCurrencyAsync(decimal amount, Currency currencyName);

        Task<decimal> ConvertFromBaseToOtherAsync(decimal amount, Currency currencyName);
    }
}