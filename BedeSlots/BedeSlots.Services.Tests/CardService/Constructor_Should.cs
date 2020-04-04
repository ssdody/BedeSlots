using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BedeSlots.Services.Tests.CardService
{
    [TestClass]
    public class Constructor_Should
    {
        [TestMethod]
        public void ThrowServiceException_WhenNullContextIsPassed()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            Assert.ThrowsException<ServiceException>(() => new Data.CardService(null, userManager));
        }
        [TestMethod]
        public void ThrowServiceException_WhenNullUserManagerIsPassed()
        {
            var bedeSlotsContext = new BedeSlotsDbContext(new Microsoft.EntityFrameworkCore.DbContextOptions<BedeSlotsDbContext>());

            Assert.ThrowsException<ServiceException>(() => new Data.CardService(bedeSlotsContext, null));
        }
        [TestMethod]
        public void NotThrowException_WhenValidParametersArePassed()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var bedeSlotsContext = new BedeSlotsDbContext(new Microsoft.EntityFrameworkCore.DbContextOptions<BedeSlotsDbContext>());

            var sut = new Data.CardService(bedeSlotsContext, userManager);
        }
    }
}
