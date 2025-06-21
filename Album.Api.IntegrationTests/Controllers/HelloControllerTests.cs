using System.Net;
using System.Net.Http.Json;
using Album.Api.Tests.IntegrationTests;
using Album.Api.Models.Dtos;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Album.Api.IntegrationTests.Controllers
{
    using AlbumModel = Album.Api.Models.Album; // voorkom 'Album is a namespace'

    public class TrackControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public TrackControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateTrack_ForExistingAlbum_ReturnsCreated()
        {
            // Arrange: voeg test-album toe
            var album = new CreateAlbumDto
            {
                Name = "Test Album",
                Artist = "Test Artist",
                ImageUrl = "http://test.com/image.png"
            };

            var albumResponse = await _client.PostAsJsonAsync("/api/Album", album);
            albumResponse.EnsureSuccessStatusCode();
            var createdAlbum = await albumResponse.Content.ReadFromJsonAsync<AlbumDto>();

            var newTrack = new CreateTrackDto
            {
                Title = "My Track",
                Artist = "Track Artist",
                Duration = 180,
                AlbumId = createdAlbum!.Id
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Track", newTrack);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<TrackDto>();
            Assert.NotNull(result);
            Assert.Equal("My Track", result!.Title);
        }

        [Fact]
        public async Task CreateTrack_ForNonExistingAlbum_ReturnsNotFound()
        {
            // Arrange
            var nonExistentAlbumId = Guid.NewGuid();

            var newTrack = new CreateTrackDto
            {
                Title = "Invalid Track",
                Artist = "Ghost Artist",
                Duration = 120,
                AlbumId = nonExistentAlbumId
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Track", newTrack);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}