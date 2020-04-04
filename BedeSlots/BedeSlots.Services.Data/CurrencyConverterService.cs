using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Services.Data.Exceptions;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data
{
    public class CurrencyConverterService : ICurrencyConverterService
    {
        private readonly IExchangeRateApiService exchangeRateApiCallService;

        public CurrencyConverterService(IExchangeRateApiService exchangeRateApiCallService)
        {
            this.exchangeRateApiCallService = exchangeRateApiCallService ?? throw new ServiceException(nameof(exchangeRateApiCallService));
        }

        public async Task<decimal> ConvertToBaseCurrencyAsync(decimal amount, Currency currencyName)
        {
            if (amount < 0)
            {
                throw new ServiceException("Amount must be a positive number!");
            }
            
            var rateToBaseCurrency = await this.exchangeRateApiCallService.GetRateAsync(currencyName);

            return amount * (1 / rateToBaseCurrency);
        }

        public async Task<decimal> ConvertFromBaseToOtherAsync(decimal amount, Currency currencyName)
        {
            if (amount < 0)
            {
                throw new ServiceException("Amount must be a positive number!");
            }

            var rateToBaseCurrency = await this.exchangeRateApiCallService.GetRateAsync(currencyName);

            return amount * rateToBaseCurrency;
        }
    }
}
