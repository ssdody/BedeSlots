using System.Threading.Tasks;
using BedeSlots.Data.Models;

namespace BedeSlots.Services.Data.Contracts
{
    public interface ICurrencyConverterService
    {
        Task<decimal> ConvertToBaseCurrency(decimal amount, Currency currencyName);

        Task<decimal> ConvertFromBaseToOther(decimal amount, Currency currencyName);
    }
}