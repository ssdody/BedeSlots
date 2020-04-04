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
using System.Security.Claims;
using System.Threading.Tasks;

namespace BedeSlots.Web.Tests.Controllers.DepositControllerTest
{
    [TestClass]
    public class Deposit_Should
    {
        [TestMethod]
        public async Task ReturnsPartialViewResult_WhenCalled()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            var userBalServiceMock = new Mock<IUserBalanceService>();
            var transactionServiceMock = new Mock<ITransactionService>();
            var userManagerMock = SetupUserManagerMock();

            var controller = SetupController(cardServiceMock, userBalServiceMock, transactionServiceMock, userManagerMock);

            var model = new DepositViewModel()
            {
                Currency = Currency.BGN,
                DepositAmount = 1
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

            var card = new CardDetailsDto()
            {
                Id = 1,
                LastFourDigit = "1234"
            };
            cardServiceMock
                .Setup(c => c.GetCardDetailsByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(card);

            // Act
            var result = await controller.Deposit(model);
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
            var userManagerMock = SetupUserManagerMock();

            var controller = SetupController(cardServiceMock, userBalServiceMock, transactionServiceMock, userManagerMock);

            var model = new DepositViewModel()
            {
                Currency = Currency.BGN,
                DepositAmount = 1
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

            var card = new CardDetailsDto()
            {
                Id = 1,
                LastFourDigit = "1234"
            };
            cardServiceMock
                .Setup(c => c.GetCardDetailsByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(card);

            // Act
            var result = await controller.Deposit(model);
            // Assert
            userBalServiceMock
                .Verify(ub => ub.DepositMoneyAsync(It.IsAny<decimal>(),
                It.IsAny<string>()), Times.Once);
        }

        private DepositController SetupController(Mock<ICardService> cardServiceMock,
            Mock<IUserBalanceService> userBalanceServiceMock,
            Mock<ITransactionService> transactionerviceMock,
            Mock<UserManager<User>> userManagerMock)
        {

            var controller = new DepositController(userManagerMock.Object,
                userBalanceServiceMock.Object,
                transactionerviceMock.Object,
                cardServiceMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal()
                    }
                },
                TempData = new Mock<ITempDataDictionary>().Object
            };;

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
