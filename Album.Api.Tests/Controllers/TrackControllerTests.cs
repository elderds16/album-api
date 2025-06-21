using Album.Api.Controllers;
using Album.Api.Data;
using Album.Api.Models;
using Album.Api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using AlbumModel = Album.Api.Models.Album;
using TrackModel = Album.Api.Models.Track;

namespace Album.Api.Tests.Controllers
{
    public class TrackControllerTests
    {
        private AlbumContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AlbumContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AlbumContext(options);
        }

        [Fact]
        public async Task CreateTrack_ValidAlbum_ReturnsCreated()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            var album = new AlbumModel
            {
                Id = Guid.NewGuid(),
                Name = "Test Album",
                Artist = "Artist",
                ImageUrl = "http://test.com"
            };
            context.Albums.Add(album);
            await context.SaveChangesAsync();

            var controller = new TrackController(context);
            var newTrack = new CreateTrackDto
            {
                Title = "Track 1",
                Artist = "Track Artist",
                Duration = 200,
                // AlbumId is niet meer nodig in DTO
            };

            // Act: roep CreateTrack aan met album.Id + DTO
            var result = await controller.CreateTrack(album.Id, newTrack);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var dto = Assert.IsType<TrackDto>(createdResult.Value);
            Assert.Equal("Track 1", dto.Title);
        }


        [Fact]
        public async Task CreateTrack_NonExistingAlbum_ReturnsNotFound()
        {
            var context = GetInMemoryDbContext();
            var controller = new TrackController(context);

            var nonExistingAlbumId = Guid.NewGuid();

            var newTrack = new CreateTrackDto
            {
                Title = "Ghost Track",
                Artist = "Nobody",
                Duration = 120,
                // AlbumId niet meer in DTO
            };

            var result = await controller.CreateTrack(nonExistingAlbumId, newTrack);

            Assert.IsType<NotFoundObjectResult>(result);
        }


        [Fact]
        public async Task UpdateTrack_ExistingTrack_ReturnsNoContent()
        {
            var context = GetInMemoryDbContext();
            var album = new AlbumModel { Id = Guid.NewGuid(), Name = "Album", Artist = "A", ImageUrl = "" };
            var track = new TrackModel { Title = "Old", Artist = "X", Duration = 100, AlbumId = album.Id };
            context.Albums.Add(album);
            context.Tracks.Add(track);
            await context.SaveChangesAsync();

            var controller = new TrackController(context);
            var updateDto = new UpdateTrackDto { Title = "New", Artist = "Y", Duration = 300 };

            var result = await controller.UpdateTrack(track.Id, updateDto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTrack_ExistingTrack_ReturnsNoContent()
        {
            var context = GetInMemoryDbContext();
            var album = new AlbumModel { Id = Guid.NewGuid(), Name = "Album", Artist = "A", ImageUrl = "" };
            var track = new TrackModel { Title = "ToDelete", Artist = "Del", Duration = 200, AlbumId = album.Id };
            context.Albums.Add(album);
            context.Tracks.Add(track);
            await context.SaveChangesAsync();

            var controller = new TrackController(context);
            var result = await controller.DeleteTrack(track.Id);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTrack_NonExistingTrack_ReturnsNotFound()
        {
            var context = GetInMemoryDbContext();
            var controller = new TrackController(context);

            var result = await controller.DeleteTrack(999);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
