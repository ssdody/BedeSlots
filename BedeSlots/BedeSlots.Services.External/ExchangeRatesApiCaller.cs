using BedeSlots.Services.External.Contracts;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BedeSlots.Services.External
{
    public class ExchangeRatesApiCaller : IExchangeRatesApiCaller
    {
        public async Task<string> GetCurrenciesRatesAsync()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(ServicesConstants.ApiBaseAddress);
                    var response = await client.GetAsync(ServicesConstants.ApiParameters);
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    return stringResult;
                }
                catch (HttpRequestException httpRequestException)
                {
                    return httpRequestException.Message;
                }
            }
        }
    }
}
