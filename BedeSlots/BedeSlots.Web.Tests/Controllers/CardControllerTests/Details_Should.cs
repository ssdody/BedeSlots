using BedeSlots.Common.Providers.Contracts;
using BedeSlots.Data.Models;
using BedeSlots.DTO.BankCardDto;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Controllers;
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

namespace BedeSlots.Web.Tests.Controllers.CardControllerTests
{
    [TestClass]
    public class Details_Should
    {
        [TestMethod]
        public async Task ReturnsPartialViewResult_WhenCalled()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            var dateTimeProvMock = new Mock<IDateTimeProvider>();
            var controller = SetupController(cardServiceMock, dateTimeProvMock);

            var card = new CardDetailsDto() {
                Id = 1,
                CardholerName = "name",
                Cvv = "123",
                ExpiryDate = DateTime.Now,
                LastFourDigit ="1234",
                Type = CardType.AmericanExpress
            };

            cardServiceMock.Setup(c => c.GetCardDetailsByIdAsync(It.IsAny<int>())).ReturnsAsync(card);

            // Act
            var result = await controller.Details(1);
            // Assert
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        public async Task InvokeCorrectServiceMethod_WhenCalled()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            var dateTimeProvMock = new Mock<IDateTimeProvider>();
            var controller = SetupController(cardServiceMock, dateTimeProvMock);

            var card = new CardDetailsDto()
            {
                Id = 1,
                CardholerName = "name",
                Cvv = "123",
                ExpiryDate = DateTime.Now,
                LastFourDigit = "1234",
                Type = CardType.AmericanExpress
            };

            cardServiceMock.Setup(c => c.GetCardDetailsByIdAsync(It.IsAny<int>())).ReturnsAsync(card);

            // Act
            var result = await controller.Details(1);

            // Assert
            cardServiceMock.Verify(x => x.GetCardDetailsByIdAsync(It.IsAny<int>()), Times.Once);
        }

        private CardController SetupController(Mock<ICardService> cardServiceMock, Mock<IDateTimeProvider> dateTimeMock)
        {
            var userManagerMock = SetupUserManagerMock();

            var controller = new CardController(userManagerMock.Object, cardServiceMock.Object, dateTimeMock.Object)
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
