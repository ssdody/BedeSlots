using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Services.Data.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BedeSlots.Services.Tests.UserService
{
    [TestClass]
    public class Constructor_Should
    {
        [TestMethod]
        public void ThrowServiceException_WhenNullContextParameterIsPassed()
        {
            var userStoreMock = new Mock<IUserStore<User>>();

            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ThrowServiceException_WhenNullContextParameterIsPassed").Options;

            var transactionServiceMock = new Mock<ITransactionService>();

            Assert.ThrowsException<ServiceException>(() => new Data.UserService(null, userManager));
        }

        [TestMethod]
        public void ThrowServiceException_WhenNullUserManagerParameterIsPassed()
        {
            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
.UseInMemoryDatabase(databaseName: "ThrowServiceException_WhenNullUserManagerParameterIsPassed").Options;

            var transactionServiceMock = new Mock<ITransactionService>();

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                Assert.ThrowsException<ServiceException>(() => new Data.UserService(bedeSlotsContext, null));
            }
        }

        [TestMethod]
        public void NotThrowException_WhenValidParametersArePassed()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "NotThrowException_WhenValidParametersArePassed").Options;

            var transactionServiceMock = new Mock<ITransactionService>();

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var sut = new Data.UserService(bedeSlotsContext, userManager);

                Assert.IsInstanceOfType(sut, typeof(Data.UserService));
            }
        }
    }
}
