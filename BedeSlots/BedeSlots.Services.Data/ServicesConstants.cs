using BedeSlots.Data.Models;

namespace BedeSlots.Services.Data
{
    public static class ServicesConstants
    {
        public const string BaseCurrencyName = "USD";
        public const string Currencies = "EUR,GBP,BGN,USD";
        public const Currency BaseCurrency = Currency.USD;

        public const string ApiBaseAddress = "https://api.exchangeratesapi.io";
        public const string ApiParameters = "/latest?base=" + BaseCurrencyName + "&symbols=" + Currencies;
    }
}
