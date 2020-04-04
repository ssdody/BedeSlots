using BedeSlots.Data.Models;
using BedeSlots.DTO.BankCardDto;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Controllers;
using BedeSlots.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BedeSlots.Web.Tests.Controllers.WithdrawControllerTests
{
    [TestClass]
    public class Withdraw_Should
    {
        [TestMethod]
        public async Task ReturnsPartialViewResult_WhenValidModelPassed()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            var userBalServiceMock = new Mock<IUserBalanceService>();
            var transactionServiceMock = new Mock<ITransactionService>();
            var currencyServiceMock = new Mock<ICurrencyService>();
            var userManagerMock = SetupUserManagerMock();

            var controller = SetupController(cardServiceMock, userBalServiceMock, transactionServiceMock, userManagerMock, currencyServiceMock);

            var model = new WithdrawViewModel()
            {
                Currency = Currency.BGN,
                Amount = 1
            };

            var user = controller.User;
            var appUser = new User()
            {
                Id = "guid",
                Currency = Currency.BGN
            };

            userManagerMock
                .Setup(u => u.GetUserAsync(user))
                .ReturnsAsync(appUser);

            var userBalance = 2m;
            userBalServiceMock
                .Setup(ub => ub.GetUserBalanceByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(userBalance);

            var card = new CardNumberDto()
            {
                Id = 1,
                Number = "1234"
            };
            cardServiceMock
                .Setup(c => c.GetCardNumberByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(card);

            currencyServiceMock
                .Setup(c => c.GetUserCurrencyAsync(It.IsAny<string>()))
                .ReturnsAsync(Currency.BGN);

            // Act
            var result = await controller.Withdraw(model);
            // Assert
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        public async Task InvokeCorrectServiceMethod_WhenCalled()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            var userBalServiceMock = new Mock<IUserBalanceService>();
            var transactionServiceMock = new Mock<ITransactionService>();
            var currencyServiceMock = new Mock<ICurrencyService>();
            var userManagerMock = SetupUserManagerMock();

            var controller = SetupController(cardServiceMock, userBalServiceMock, transactionServiceMock, userManagerMock, currencyServiceMock);

            var model = new WithdrawViewModel()
            {
                Currency = Currency.BGN,
                Amount = 1
            };

            var user = controller.User;
            var appUser = new User()
            {
                Id = "guid",
                Currency = Currency.BGN
            };

            userManagerMock
                .Setup(u => u.GetUserAsync(user))
                .ReturnsAsync(appUser);

            var userBalance = 2m;
            userBalServiceMock
                .Setup(ub => ub.GetUserBalanceByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(userBalance);

            var card = new CardNumberDto()
            {
                Id = 1,
                Number = "1234"
            };
            cardServiceMock
                .Setup(c => c.GetCardNumberByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(card);

            currencyServiceMock
                .Setup(c => c.GetUserCurrencyAsync(It.IsAny<string>()))
                .ReturnsAsync(Currency.BGN);

            // Act
            var result = await controller.Withdraw(model);
            // Assert
            userBalServiceMock
                .Verify(ub => ub.ReduceMoneyAsync(It.IsAny<decimal>(),
                It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task ReturnsPartialViewResult_WhenInvalidAmountPassed()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            var userBalServiceMock = new Mock<IUserBalanceService>();
            var transactionServiceMock = new Mock<ITransactionService>();
            var currencyServiceMock = new Mock<ICurrencyService>();
            var userManagerMock = SetupUserManagerMock();

            var controller = SetupController(cardServiceMock, userBalServiceMock, transactionServiceMock, userManagerMock, currencyServiceMock);

            var model = new WithdrawViewModel()
            {
                Currency = Currency.BGN,
                Amount = 2
            };

            var user = controller.User;
            var appUser = new User()
            {
                Id = "guid",
                Currency = Currency.BGN
            };

            userManagerMock
                .Setup(u => u.GetUserAsync(user))
                .ReturnsAsync(appUser);

            var userBalance = 1m;
            userBalServiceMock
                .Setup(ub => ub.GetUserBalanceByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(userBalance);

            // Act
            var result = await controller.Withdraw(model);
            // Assert
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        private WithdrawController SetupController(Mock<ICardService> cardServiceMock,
           Mock<IUserBalanceService> userBalanceServiceMock,
           Mock<ITransactionService> transactionerviceMock,
           Mock<UserManager<User>> userManagerMock,
           Mock<ICurrencyService> curencyServiceMock)
        {
            var controller = new WithdrawController(
                userBalanceServiceMock.Object,
                transactionerviceMock.Object,
                cardServiceMock.Object,
                curencyServiceMock.Object,
                userManagerMock.Object)
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
