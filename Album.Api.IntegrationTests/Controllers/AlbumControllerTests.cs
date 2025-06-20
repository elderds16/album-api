using Album.Api.Data;
using Album.Api.Models.Dtos;
using Album.Api.Tests.IntegrationTests;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using AlbumModel = Album.Api.Models.Album;

namespace Album.Api.IntegrationTests.Controllers
{
    public class AlbumControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public AlbumControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAlbums_ReturnsOkResult()
        {
            // Arrange
            await AddTestAlbumsAsync();

            // Act
            var response = await _client.GetAsync("/api/Album");
            response.EnsureSuccessStatusCode();
            var albums = await response.Content.ReadFromJsonAsync<List<AlbumDto>>();

            // Assert
            Assert.NotNull(albums);
            Assert.True(albums.Count > 0);
        }

        [Fact]
        public async Task GetAlbum_ReturnsOkResult_WithAlbum()
        {
            // Arrange
            var albumId = await AddTestAlbumAsync();

            // Act
            var response = await _client.GetAsync($"/api/Album/{albumId}");
            response.EnsureSuccessStatusCode();
            var album = await response.Content.ReadFromJsonAsync<AlbumDto>();

            // Assert
            Assert.NotNull(album);
            Assert.Equal(albumId, album.Id);
        }

        [Fact]
        public async Task GetAlbum_ReturnsNotFound_ForInvalidId()
        {
            // Act
            var response = await _client.GetAsync($"/api/Album/{Guid.NewGuid()}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PostAlbum_CreatesNewAlbum()
        {
            // Arrange
            var createAlbumDto = new CreateAlbumDto
            {
                Name = "New Album",
                Artist = "New Artist",
                ImageUrl = "http://newimage.com"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Album", createAlbumDto);
            response.EnsureSuccessStatusCode();
            var createdAlbum = await response.Content.ReadFromJsonAsync<AlbumDto>();

            // Assert
            Assert.NotNull(createdAlbum);
            Assert.Equal(createAlbumDto.Name, createdAlbum.Name);
        }

        [Fact]
        public async Task PutAlbum_UpdatesExistingAlbum()
        {
            // Arrange
            var albumId = await AddTestAlbumAsync();
            var updateAlbumDto = new UpdateAlbumDto
            {
                Name = "Updated Album",
                Artist = "Updated Artist",
                ImageUrl = "http://updatedimage.com"
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/Album/{albumId}", updateAlbumDto);
            response.EnsureSuccessStatusCode();

            // Assert
            var updatedAlbumResponse = await _client.GetAsync($"/api/Album/{albumId}");
            var updatedAlbum = await updatedAlbumResponse.Content.ReadFromJsonAsync<AlbumDto>();
            Assert.NotNull(updatedAlbum);
            Assert.Equal(updateAlbumDto.Name, updatedAlbum.Name);
        }

        [Fact]
        public async Task PutAlbum_ReturnsNotFound_WhenAlbumNotFound()
        {
            // Arrange
            var updateAlbumDto = new UpdateAlbumDto
            {
                Name = "Newly Updated Album for not found",
                Artist = "Updated Artist",
                ImageUrl = "http://updatedimage.com"
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/Album/{Guid.NewGuid()}", updateAlbumDto);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PutAlbum_ReturnsNotFound_WhenDbUpdateConcurrencyExceptionThrown()
        {
            // Arrange
            var albumId = await AddTestAlbumAsync();
            var updateAlbumDto = new UpdateAlbumDto
            {
                Name = "Newly Updated Album for ConcException",
                Artist = "Updated Artist",
                ImageUrl = "http://updatedimage.com"
            };

            // Simulate DbUpdateConcurrencyException by removing the album after fetching it.
            await RemoveAlbumAsync(albumId);

            // Act
            var response = await _client.PutAsJsonAsync($"/api/Album/{albumId}", updateAlbumDto);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteAlbum_RemovesAlbum()
        {
            // Arrange
            var albumId = await AddTestAlbumAsync();

            // Act
            var response = await _client.DeleteAsync($"/api/Album/{albumId}");
            response.EnsureSuccessStatusCode();

            // Assert
            var deletedAlbumResponse = await _client.GetAsync($"/api/Album/{albumId}");
            Assert.Equal(HttpStatusCode.NotFound, deletedAlbumResponse.StatusCode);
        }

        [Fact]
        public async Task DeleteAlbum_ReturnsNotFound_WhenAlbumNotFound()
        {
            // Act
            var response = await _client.DeleteAsync($"/api/Album/{Guid.NewGuid()}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PostAlbum_ReturnsBadRequest_WhenAlbumAlreadyExists()
        {
            // Arrange
            await AddTestAlbumAsync();

            var createAlbumDto = new CreateAlbumDto
            {
                Name = "Test Album",
                Artist = "Test Artist",
                ImageUrl = "http://testimage.com"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Album", createAlbumDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("An album with the same name and artist already exists.", responseContent);
        }

        [Fact]
        public async Task PutAlbum_ReturnsBadRequest_WhenAlbumAlreadyExists()
        {
            // Arrange
            await AddTestAlbumsAsync();
            var albumId = await AddTestAlbumAsync();
            var updateAlbumDto = new UpdateAlbumDto
            {
                Name = "Album1",
                Artist = "Artist1",
                ImageUrl = "http://testimage.com"
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/Album/{albumId}", updateAlbumDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("An album with the same name and artist already exists.", responseContent);
        }

        private async Task<Guid> AddTestAlbumAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AlbumContext>();
            var album = new AlbumModel
            {
                Id = Guid.NewGuid(),
                Name = "Test Album",
                Artist = "Test Artist",
                ImageUrl = "http://testimage.com"
            };
            context.Albums.Add(album);
            await context.SaveChangesAsync();
            return album.Id;
        }

        private async Task AddTestAlbumsAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AlbumContext>();
            var albums = new List<AlbumModel>
                {
                    new () { Id = Guid.NewGuid(), Name = "Album1", Artist = "Artist1", ImageUrl = "http://image1.com" },
                    new () { Id = Guid.NewGuid(), Name = "Album2", Artist = "Artist2", ImageUrl = "http://image2.com" }
                };
            context.Albums.AddRange(albums);
            await context.SaveChangesAsync();
        }

        private async Task RemoveAlbumAsync(Guid albumId)
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AlbumContext>();
            var album = await context.Albums.FindAsync(albumId);
            if (album != null)
            {
                context.Albums.Remove(album);
                await context.SaveChangesAsync();
            }
        }
    }
}