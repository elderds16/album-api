using Album.Api.Services;
using System.Net;

namespace Album.Api.Tests.Services
{

    public class GreetingServiceTests
    {

        [Fact]
        public void GetGreeting_WithNullName_ReturnsHelloWorld()
        {
            // Arrange
            var service = new GreetingService();
            var hostname = Dns.GetHostName();

            // Act
            var result = service.GetGreeting(null);

            // Assert
            Assert.Equal($"Hello World from {hostname} v2", result.Message);
        }

        [Fact]
        public void GetGreeting_WithName_ReturnsHelloName()
        {
            // Arrange
            var service = new GreetingService();
            var hostname = Dns.GetHostName();

            // Act
            var result = service.GetGreeting("Test");

            // Assert
            Assert.Equal($"Hello Test from {hostname} v2", result.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void GetGreeting_WithInvalidName_ReturnsHelloWorld(string? name)
        {
            // Arrange
            var service = new GreetingService();
            var hostname = Dns.GetHostName();

            // Act
            var result = service.GetGreeting(name);

            // Assert
            Assert.Equal($"Hello World from {hostname} v2", result.Message);
        }

    }
}
