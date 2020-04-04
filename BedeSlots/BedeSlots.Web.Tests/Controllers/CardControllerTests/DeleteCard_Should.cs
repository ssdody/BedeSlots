using BedeSlots.Common.Providers.Contracts;
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

namespace BedeSlots.Web.Tests.Controllers.CardControllerTests
{
    [TestClass]
    public class DeleteCard_Should
    {
        [TestMethod]
        public async Task InvokeCorrectServiceMethod_WhenCalled()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            var dateTimeProvMock = new Mock<IDateTimeProvider>();
            var controller = SetupController(cardServiceMock, dateTimeProvMock);

            // Act
            var result = await controller.Delete(1);

            // Assert
            cardServiceMock.Verify(x => x.DeleteCardAsync(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public async Task ReturnRedirectResult_WhenCalled()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            var dateTimeProvMock = new Mock<IDateTimeProvider>();
            var controller = SetupController(cardServiceMock, dateTimeProvMock);

            // Act
            var result = await controller.Delete(1);

            //  Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.AreEqual("Deposit", redirectResult.ControllerName);
            Assert.IsNull(redirectResult.RouteValues);
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
