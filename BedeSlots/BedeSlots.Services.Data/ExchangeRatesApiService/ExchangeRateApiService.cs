using BedeSlots.Data.Models;
using BedeSlots.DTO.ExchangeRatesDto;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Services.External.Contracts;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data
{
    public class ExchangeRateApiService : IExchangeRateApiService
    {
        private readonly IExchangeRatesApiCaller exchangeRateApiCaller;
        private readonly IMemoryCache cache;
        private readonly string key = "RatesKey";

        public ExchangeRateApiService(IExchangeRatesApiCaller exchangeRateApiCaller, IMemoryCache cache)
        {
            this.exchangeRateApiCaller = exchangeRateApiCaller;
            this.cache = cache;
        }

        public async Task<IDictionary<Currency, decimal>> GetAllRatesAsync()
        {
            if (cache.TryGetValue(key, out IDictionary<Currency, decimal> rates))
            {
                return rates;
            }

            var stringResultRates = await this.exchangeRateApiCaller.GetCurrenciesRatesAsync(ServicesConstants.ApiBaseAddress, ServicesConstants.ApiParameters);
            var deserializedRates = JsonConvert.DeserializeObject<CurrencyDto>(stringResultRates);

            var currencies = Enum.GetValues(typeof(Currency)).Cast<Currency>().ToList();

            rates = new Dictionary<Currency, decimal>
            {
                { Currency.BGN, decimal.Parse(deserializedRates.Rates.BGN)},
                { Currency.EUR, decimal.Parse(deserializedRates.Rates.EUR)},
                { Currency.GBP, decimal.Parse(deserializedRates.Rates.GBP)},
                { Currency.USD, decimal.Parse(deserializedRates.Rates.USD )}
            };

            var cacheEntryOptions = new MemoryCacheEntryOptions()
           .SetAbsoluteExpiration(TimeSpan.FromSeconds(60 * 60 * 24));

            cache.Set(key, rates, cacheEntryOptions);

            return rates;
        }

        public async Task<decimal> GetRateAsync(Currency currencyName)
        {
            var rates = await GetAllRatesAsync();
            return rates[currencyName];
        }
    }
}
