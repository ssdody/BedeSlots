using BedeSlots.Common.Providers.Contracts;
using BedeSlots.Games.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BedeSlots.Games.Tests.SlotMachineTests
{
    [TestClass]
    public class Spin_Should
    {
        [TestMethod]
        public void ReturnSpinDataInstance_WhenInvoked()
        {
            // Arrange
            int rows = 4;
            int cols = 4;
            decimal amount = 12.12m;
            var randomProviderMock = new Mock<IRandomProvider>();
            var sut = new SlotMachine(randomProviderMock.Object);

            // Act
            var result = sut.Spin(rows, cols, amount);

            // Assert
            Assert.IsInstanceOfType(result, typeof(SpinData));
        }
    }
}
