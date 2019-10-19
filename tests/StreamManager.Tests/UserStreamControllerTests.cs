using NUnit.Framework;
using Moq;
using StreamManager.Repositories;
using System.Threading.Tasks;
using StreamManager.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace StreamManager.Tests
{
    public class UserStreamControllerTests
    {
        [Test]
        public async Task UserStreamControllerTests_CountReturnsCountFromRepository()
        {
            //Arrange
            var mockRepository = new Mock<IUserStreamRepository>();
            mockRepository.Setup(r => r.GetUserStreamCount(It.IsAny<string>())).Returns(() => Task.FromResult(3));
            var mockLogger = new Mock<ILogger<UserStreamController>>();
            var userStreamController = new UserStreamController(mockRepository.Object, mockLogger.Object);

            //Act
            var userStreamCount = await userStreamController.Count("test-user") as OkObjectResult;

            //Assert
            Assert.IsNotNull(userStreamCount);
            Assert.AreEqual(3, userStreamCount.Value);
        }
    }
}