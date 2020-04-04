using System.Threading.Tasks;

namespace BedeSlots.Services.External.Contracts
{
    public interface IExchangeRatesApiCaller
    {
        Task<string> GetCurrenciesRatesAsync(string baseAddress, string parameters);
    }
}