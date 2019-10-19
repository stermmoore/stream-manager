using NUnit.Framework;
using Moq;
using StreamManager.Repositories;
using System.Threading.Tasks;
using StreamManager.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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
            var appSettingsMock = new Mock<IOptions<AppSettings>>();
            var userStreamController = new UserStreamController(mockRepository.Object, appSettingsMock.Object, mockLogger.Object);

            //Act
            var userStreamCount = await userStreamController.Count("test-user") as OkObjectResult;

            //Assert
            Assert.IsNotNull(userStreamCount);
            Assert.AreEqual(3, userStreamCount.Value);
        }

                [Test]
        public async Task UserStreamControllerTests_StartIncrementsStreamCount()
        {
            //Arrange
            var mockRepository = new Mock<IUserStreamRepository>();
            mockRepository.Setup(r => r.GetUserStreamCount(It.IsAny<string>())).Returns(() => Task.FromResult(2));
            var mockLogger = new Mock<ILogger<UserStreamController>>();
            var appSettingsMock = new Mock<IOptions<AppSettings>>();
            appSettingsMock.Setup(s => s.Value).Returns(() => new AppSettings{ MaximumConcurrentUserStreams=3 });
            var userStreamController = new UserStreamController(mockRepository.Object, appSettingsMock.Object, mockLogger.Object);

            //Act
            var userStreamStartResponse = await userStreamController.Start("test-user") as OkResult;

            //Assert
            Assert.IsNotNull(userStreamStartResponse);
            mockRepository.Verify(r => r.IncrementUserStreamCount(It.IsAny<string>()), Times.Once);
        }
    }
}