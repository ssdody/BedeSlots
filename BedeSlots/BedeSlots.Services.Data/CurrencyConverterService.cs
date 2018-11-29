using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data
{
    public class CurrencyConverterService : ICurrencyConverterService
    {
        private readonly IExchangeRateApiCallService exchangeRateApiCallService;

        public CurrencyConverterService(IExchangeRateApiCallService exchangeRateApiCallService)
        {
            this.exchangeRateApiCallService = exchangeRateApiCallService;
        }

        public async Task<decimal> ConvertToBaseCurrency(decimal amount, Currency currencyName)
        {
            var rateToBaseCurrency = await this.exchangeRateApiCallService.GetRateAsync(currencyName);

            return amount * (1 / rateToBaseCurrency);
        }

        public async Task<decimal> ConvertFromBaseToOther(decimal amount, Currency currencyName)
        {
            var rateToBaseCurrency = await this.exchangeRateApiCallService.GetRateAsync(currencyName);

            return amount * rateToBaseCurrency;
        }
    }
}
