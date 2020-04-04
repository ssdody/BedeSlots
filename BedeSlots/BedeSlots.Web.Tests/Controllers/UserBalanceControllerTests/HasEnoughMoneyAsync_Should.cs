using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BedeSlots.Web.Tests.Controllers.UserBalanceControllerTests
{
    [TestClass]
    public class HasEnoughMoneyAsync_Should
    {
        [TestMethod]
        public async Task ReturnsJsonTrue_WhenValidAmountPassed()
        {
            // Arrange
            var userBalanceServiceMock = new Mock<IUserBalanceService>();
            var userManagerMock = SetupUserManagerMock();
            var controller = SetupController(userBalanceServiceMock, userManagerMock);

            var user = controller.User;
            var appUser = new User()
            {
                Id = "userId",
            };

            userManagerMock
                .Setup(u => u.GetUserAsync(user))
                .ReturnsAsync(appUser);

            decimal userbalance = 2;
            userBalanceServiceMock
                .Setup(ub => ub.GetUserBalanceByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(userbalance);

            // Act
            decimal amount = 1;
            var result = await controller.HasEnoughMoneyAsync(amount);

            // Assert
            Assert.AreEqual(true, result.Value);
        }

        [TestMethod]
        public async Task ReturnsErrorMessage_WhenInvalidAmountPassed()
        {
            // Arrange
            var userBalanceServiceMock = new Mock<IUserBalanceService>();
            var userManagerMock = SetupUserManagerMock();
            var controller = SetupController(userBalanceServiceMock, userManagerMock);

            var user = controller.User;
            var appUser = new User()
            {
                Id = "userId",
            };

            userManagerMock
                .Setup(u => u.GetUserAsync(user))
                .ReturnsAsync(appUser);

            decimal userbalance = 1;
            userBalanceServiceMock
                .Setup(ub => ub.GetUserBalanceByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(userbalance);

            // Act
            decimal amount = 2;
            var result = await controller.HasEnoughMoneyAsync(amount);
            string resultMsg = result.Value.ToString();

            // Assert
            StringAssert.Contains(resultMsg, "don't have enough");
        }


        private UserBalanceController SetupController(Mock<IUserBalanceService> userBalanceServiceMock, Mock<UserManager<User>> userManagerMock)
        {
            var controller = new UserBalanceController(userBalanceServiceMock.Object, userManagerMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal()
                    }
                },
                TempData = new Mock<ITempDataDictionary>().Object
            }; ;

            return controller;
        }

        private Mock<UserManager<User>> SetupUserManagerMock()
        {
            return new Mock<UserManager<User>>(
                  new Mock<IUserStore<User>>().Object,
                  null,
                  null,
                  null,
                  null,
                  null,
                  null,
                  null,
                  null);
        }
    }
}
