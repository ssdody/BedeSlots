using System.Collections.Generic;
using BedeSlots.Data.Models;

namespace BedeSlots.Services.Data.Contracts
{
    public interface ICurrencyService
    {
        ICollection<Currency> GetAllCurrenciesNames();
    }
}