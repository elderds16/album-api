using Album.Api.Controllers;
using Album.Api.Interfaces;
using Album.Api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Album.Api.Tests.Controllers;

public class AlbumControllerTests
{
    private readonly Mock<IAlbumService> _mockAlbumService;
    private readonly AlbumController _albumController;

    public AlbumControllerTests()
    {
        _mockAlbumService = new Mock<IAlbumService>();
        _albumController = new AlbumController(_mockAlbumService.Object);
    }

    [Fact]
    public async Task GetAlbums_ShouldReturnOkResultWithAlbums()
    {
        // Arrange
        var albums = new List<AlbumDto>
            {
                new () { Id = Guid.NewGuid(), Name = "Album1", Artist = "Artist1", ImageUrl = "http://image1.com" },
                new () { Id = Guid.NewGuid(), Name = "Album2", Artist = "Artist2", ImageUrl = "http://image2.com" }
            };
        _mockAlbumService.Setup(service => service.GetAllAlbumsAsync()).ReturnsAsync(albums);

        // Act
        var result = await _albumController.GetAlbums();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnAlbums = Assert.IsType<List<AlbumDto>>(okResult.Value);
        Assert.Equal(2, returnAlbums.Count);
    }

    [Fact]
    public async Task GetAlbum_ShouldReturnOkResultWithAlbum()
    {
        // Arrange
        var albumId = Guid.NewGuid();
        var album = new AlbumDto { Id = albumId, Name = "Album1", Artist = "Artist1", ImageUrl = "http://image1.com" };
        _mockAlbumService.Setup(service => service.GetAlbumByIdAsync(albumId)).ReturnsAsync(album);

        // Act
        var result = await _albumController.GetAlbum(albumId);

        // Assert
        var okResult = Assert.IsType<ActionResult<AlbumDto>>(result);
        var returnAlbum = Assert.IsType<AlbumDto>(okResult.Value);
        Assert.Equal(albumId, returnAlbum.Id);
    }

    [Fact]
    public async Task GetAlbum_ShouldReturnNotFoundResult()
    {
        // Arrange
        var albumId = Guid.NewGuid();
        _mockAlbumService.Setup(service => service.GetAlbumByIdAsync(albumId)).ReturnsAsync((AlbumDto)null!);

        // Act
        var result = await _albumController.GetAlbum(albumId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PutAlbum_ShouldReturnNoContentResult()
    {
        // Arrange
        var albumId = Guid.NewGuid();
        var updateAlbumDto = new UpdateAlbumDto { Name = "New Name", Artist = "New Artist", ImageUrl = "http://newimage.com" };

        _mockAlbumService.Setup(service => service.UpdateAlbumAsync(albumId, updateAlbumDto)).Returns(Task.CompletedTask);

        // Act
        var result = await _albumController.PutAlbum(albumId, updateAlbumDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task PutAlbum_ShouldReturnNotFoundResult_WhenKeyNotFoundExceptionThrown()
    {
        // Arrange
        var albumId = Guid.NewGuid();
        var updateAlbumDto = new UpdateAlbumDto { Name = "New Name", Artist = "New Artist", ImageUrl = "http://newimage.com" };

        _mockAlbumService.Setup(service => service.UpdateAlbumAsync(albumId, updateAlbumDto)).Throws<KeyNotFoundException>();

        // Act
        var result = await _albumController.PutAlbum(albumId, updateAlbumDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task PutAlbum_ShouldReturnNotFoundResult_WhenDbUpdateConcurrencyExceptionThrownAndAlbumNotFound()
    {
        // Arrange
        var albumId = Guid.NewGuid();
        var updateAlbumDto = new UpdateAlbumDto { Name = "New Name", Artist = "New Artist", ImageUrl = "http://newimage.com" };

        _mockAlbumService.Setup(service => service.UpdateAlbumAsync(albumId, updateAlbumDto)).Throws<DbUpdateConcurrencyException>();
        _mockAlbumService.Setup(service => service.GetAlbumByIdAsync(albumId)).ReturnsAsync((AlbumDto)null!);

        // Act
        var result = await _albumController.PutAlbum(albumId, updateAlbumDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task PutAlbum_ShouldReturnBadRequest_WhenArgumentExceptionThrown()
    {
        // Arrange
        var albumId = Guid.NewGuid();
        var updateAlbumDto = new UpdateAlbumDto { Name = "New Name", Artist = "New Artist", ImageUrl = "http://newimage.com" };
        var errorMessage = "An album with the same name and artist already exists.";

        _mockAlbumService.Setup(service => service.UpdateAlbumAsync(albumId, updateAlbumDto))
            .Throws(new ArgumentException(errorMessage));

        // Act
        var result = await _albumController.PutAlbum(albumId, updateAlbumDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(errorMessage, badRequestResult.Value);
    }

    [Fact]
    public async Task PostAlbum_ShouldReturnCreatedAtActionResult()
    {
        // Arrange
        var createAlbumDto = new CreateAlbumDto { Name = "New Album", Artist = "New Artist", ImageUrl = "http://newimage.com" };
        var albumDto = new AlbumDto { Id = Guid.NewGuid(), Name = "New Album", Artist = "New Artist", ImageUrl = "http://newimage.com" };

        _mockAlbumService.Setup(service => service.CreateAlbumAsync(createAlbumDto)).ReturnsAsync(albumDto);

        // Act
        var result = await _albumController.PostAlbum(createAlbumDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnAlbum = Assert.IsType<AlbumDto>(createdAtActionResult.Value);
        Assert.Equal(albumDto.Id, returnAlbum.Id);
    }

    [Fact]
    public async Task PostAlbum_ShouldReturnBadRequest_WhenArgumentExceptionThrown()
    {
        // Arrange
        var createAlbumDto = new CreateAlbumDto { Name = "New Album", Artist = "New Artist", ImageUrl = "http://newimage.com" };
        var errorMessage = "An album with the same name and artist already exists.";

        _mockAlbumService.Setup(service => service.CreateAlbumAsync(createAlbumDto))
            .Throws(new ArgumentException(errorMessage));

        // Act
        var result = await _albumController.PostAlbum(createAlbumDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(errorMessage, badRequestResult.Value);
    }

    [Fact]
    public async Task DeleteAlbum_ShouldReturnNoContentResult()
    {
        // Arrange
        var albumId = Guid.NewGuid();
        var album = new AlbumDto { Id = albumId, Name = "Album1", Artist = "Artist1", ImageUrl = "http://image1.com" };

        _mockAlbumService.Setup(service => service.GetAlbumByIdAsync(albumId)).ReturnsAsync(album);
        _mockAlbumService.Setup(service => service.DeleteAlbumAsync(albumId)).Returns(Task.CompletedTask);

        // Act
        var result = await _albumController.DeleteAlbum(albumId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteAlbum_ShouldReturnNotFoundResult()
    {
        // Arrange
        var albumId = Guid.NewGuid();

        _mockAlbumService.Setup(service => service.GetAlbumByIdAsync(albumId)).ReturnsAsync((AlbumDto)null!);

        // Act
        var result = await _albumController.DeleteAlbum(albumId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}