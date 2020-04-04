using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Services.Data.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedeSlots.Services.Tests.UserService
{
    [TestClass]
    public class GetUserRoleAsync_Should
    {
        private ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

        [TestMethod]
        public async Task ReturnUserRole_WhenValidParameterIsPassed()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ReturnUserRole_WhenValidParameterIsPassed")
     .UseInternalServiceProvider(serviceProvider).Options;

            var transactionServiceMock = new Mock<ITransactionService>();

            var user = new User();

            var role = new IdentityRole("User");

            IdentityUserRole<string> userRole;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                bedeSlotsContext.Roles.Add(role);
                bedeSlotsContext.Users.Add(user);
                bedeSlotsContext.SaveChanges();

                userRole = new IdentityUserRole<string>() { UserId = user.Id, RoleId = role.Id };

                bedeSlotsContext.UserRoles.Add(userRole);
                bedeSlotsContext.SaveChanges();
            }
            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var sut = new Data.UserService(bedeSlotsContext,
                    userManager);

                var result = await sut.GetUserRoleAsync(user.Id);

                Assert.IsTrue(result.RoleId == userRole.RoleId);
            }
        }

        [TestMethod]
        public async Task ThrowServiceException_WhenNullArgumentIsPassed()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ThrowServiceException_WhenNullArgumentIsPassed")
     .UseInternalServiceProvider(serviceProvider).Options;

            var transactionServiceMock = new Mock<ITransactionService>();

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var sut = new Data.UserService(bedeSlotsContext, userManager);

                await Assert.ThrowsExceptionAsync<ServiceException>(async () => await sut.GetUserRoleAsync(null));
            }
        }
    }
}
