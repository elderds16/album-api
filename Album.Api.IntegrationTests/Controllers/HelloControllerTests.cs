using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace Album.Api.IntegrationTests.Controllers
{
    public class HelloControllerTests : IClassFixture<WebApplicationFactory<Program>>

    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public HelloControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }


        [Theory]
        [InlineData(null, "Hello World")]
        [InlineData("Test", "Hello Test")]
        [InlineData("", "Hello World")]
        [InlineData(" ", "Hello World")]
        public async Task Get_HelloEndpoint_ReturnsExpectedResponse(string? name, string baseExpectedMessage)
        {
            // Arrange
            var url = name == null ? "api/hello" : $"/api/hello?name={name}";
            var hostname = Dns.GetHostName();
            var expectedMessage = $"{baseExpectedMessage} from {hostname} v2";

            // Act 
            var response = await _client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<GreetingResponse>();
            result.Should().NotBeNull();
            result!.Message.Should().Be(expectedMessage);
        }


        private class GreetingResponse
        {
            public string Message { get; set; } = null!;
        }
    }
}
