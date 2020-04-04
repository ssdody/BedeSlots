using BedeSlots.Data;
using BedeSlots.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BedeSlots.Services.Tests.CardService
{
    [TestClass]
    public class AddCardAsync_Should
    {
        [TestMethod]
        public async Task AddCardToDatabase_WhenValidParametersArePassed()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "RemoveCarSuccessfully_WhenValidParamatersArePassed").Options;

            string userId = "test";
            string cvvNumber = "123";
            string number = "1616161616161616";
            CardType type = CardType.Visa;
            DateTime createdOn = DateTime.Now;
            DateTime expiryDate = DateTime.Parse("11-11-2111");
            string cardholerName = "test";

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var sut = new Data.CardService(bedeSlotsContext, userManager);
                await sut.AddCardAsync(number, cardholerName, cvvNumber, expiryDate, type, userId);
            }

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                Assert.IsTrue(await bedeSlotsContext.BankCards.CountAsync() == 1);
                Assert.IsTrue(await bedeSlotsContext.BankCards.AnyAsync(c => c.Number == number));
            }
        }
    }
}
