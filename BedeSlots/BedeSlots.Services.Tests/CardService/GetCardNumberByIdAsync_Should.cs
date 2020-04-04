using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.DTO.BankCardDto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace BedeSlots.Services.Tests.CardService
{
    [TestClass]
    public class GetCardNumberByIdAsync_Should
    {
        private ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

        [TestMethod]
        public async Task ReturnCardNumberDto_WhenValidId()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
                .UseInMemoryDatabase(databaseName: "ReturnCardNumberDto_WhenValidId")
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            var user = new User();
            var card = new BankCard()
            {
                Id = 1,
                Number = "1111111111111111"
            };
            CardNumberDto result;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var sut = new Data.CardService(bedeSlotsContext, userManager);
                await bedeSlotsContext.BankCards.AddAsync(card);
                await bedeSlotsContext.SaveChangesAsync();

                result = await sut.GetCardNumberByIdAsync(card.Id);
            }

            Assert.IsTrue(result.Number == card.Number.Substring(12));
            Assert.IsInstanceOfType(result, typeof(CardNumberDto));
        }
    }
}
