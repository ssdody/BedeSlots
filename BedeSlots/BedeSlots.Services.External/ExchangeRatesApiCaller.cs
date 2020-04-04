using BedeSlots.Services.External.Contracts;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BedeSlots.Services.External
{
    public class ExchangeRatesApiCaller : IExchangeRatesApiCaller
    {
        public async Task<string> GetCurrenciesRatesAsync(string baseAddress, string parameters)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(baseAddress);
                    var response = await client.GetAsync(parameters);
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    return stringResult;
                }
                catch (HttpRequestException httpRequestException)
                {
                    return $"The api is not available" + httpRequestException.Message;
                }
            }
        }
    }
}
