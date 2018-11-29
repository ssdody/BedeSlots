using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BedeSlots.Services.Data
{
    public class CurrencyService : ICurrencyService
    {
        public CurrencyService()
        {
        }

        public ICollection<Currency> GetAllCurrenciesNames()
        {
           return Enum.GetValues(typeof(Currency)).Cast<Currency>().ToList();
        }
    }
}
