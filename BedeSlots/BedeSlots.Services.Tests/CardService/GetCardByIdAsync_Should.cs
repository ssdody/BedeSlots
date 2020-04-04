using BedeSlots.Data;
using BedeSlots.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BedeSlots.Services.Tests.CardService
{
    [TestClass]
    public class GetCardByIdAsync_Should
    {
        private ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

        [TestMethod]
        public async Task ReturnCardById_WhenValidIdIsPassed()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ReturnCardById_WhenValidIdIsPassed").UseInternalServiceProvider(serviceProvider)
     .Options;

            Data.CardService cardService;

            var user = new User();
            var card = new BankCard()
            {
                UserId = user.Id,
                User = user,
                CvvNumber = "123",
                Number = "1616161616161616",
                Type = CardType.Visa,
                CreatedOn = DateTime.Now
            };

            BankCard result;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                cardService = new Data.CardService(bedeSlotsContext, userManager);
                await bedeSlotsContext.BankCards.AddAsync(card);
                await bedeSlotsContext.SaveChangesAsync();

                result = await cardService.GetCardByIdAsync(card.Id);
            }

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                Assert.IsTrue(result.Id == card.Id);
            }

        }
    }
}
