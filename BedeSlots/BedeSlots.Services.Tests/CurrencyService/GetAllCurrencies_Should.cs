using BedeSlots.Data;
using BedeSlots.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BedeSlots.Services.Tests.CurrencyService
{
    [TestClass]
    public class GetAllCurrencies_Should
    {

        [TestMethod]
        public void ReturnCollectionOfAllCurrencies_WhenInvoked()
        {
            var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

            var contexOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
                 .UseInMemoryDatabase(databaseName: "ReturnCollectionOfAllCurrencies_WhenInvoked")
                 .UseInternalServiceProvider(serviceProvider).Options;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contexOptions))
            {
                var sut = new Data.CurrencyService(bedeSlotsContext);
                var result = sut.GetAllCurrencies();

                Assert.IsInstanceOfType(result, typeof(ICollection<Currency>));
                Assert.IsTrue(result.Count == 4);
            }
        }
    }
}
