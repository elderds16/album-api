﻿using Album.Api.Interfaces;
using Album.Api.Models.Dtos;
using Album.Api.Services;
using Moq;
using AlbumModel = Album.Api.Models.Album;
using Microsoft.EntityFrameworkCore;
using Album.Api.Data;
using Album.Api.Models;


namespace Album.Api.Tests.Services;

public class AlbumServiceTests
{
    private readonly Mock<IAlbumRepository> _mockRepository;
    private readonly AlbumService _albumService;
    private readonly AlbumContext _context;

    public AlbumServiceTests()
    {
        _mockRepository = new Mock<IAlbumRepository>();

        var options = new DbContextOptionsBuilder<AlbumContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AlbumContext(options);
        _context.Database.EnsureCreated();

        _albumService = new AlbumService(_mockRepository.Object, _context);
    }

    [Fact]
    public async Task CreateAlbumAsync_ShouldReturnAlbumDto()
    {
        // Arrange
        var createAlbumDto = new CreateAlbumDto
        {
            Name = "Test Album",
            Artist = "Test Artist",
            ImageUrl = "http://testimage.com"
        };

        var album = new AlbumModel
        {
            Id = Guid.NewGuid(),
            Name = createAlbumDto.Name,
            Artist = createAlbumDto.Artist,
            ImageUrl = createAlbumDto.ImageUrl
        };

        _mockRepository.Setup(r => r.CreateAlbumAsync(It.IsAny<AlbumModel>())).ReturnsAsync(album);

        // Act
        var result = await _albumService.CreateAlbumAsync(createAlbumDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(album.Id, result.Id);
        Assert.Equal(createAlbumDto.Name, result.Name);
        Assert.Equal(createAlbumDto.Artist, result.Artist);
        Assert.Equal(createAlbumDto.ImageUrl, result.ImageUrl);
    }

    [Fact]
    public async Task CreateAlbumAsync_ShouldThrowArgumentException_WhenAlbumAlreadyExists()
    {
        // Arrange
        var createAlbumDto = new CreateAlbumDto
        {
            Name = "Test Album",
            Artist = "Test Artist",
            ImageUrl = "http://testimage.com"
        };

        _mockRepository.Setup(r => r.AlbumExistsByNameAndArtistAsync(createAlbumDto.Name, createAlbumDto.Artist)).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _albumService.CreateAlbumAsync(createAlbumDto));
    }

    [Fact]
    public async Task DeleteAlbumAsync_ShouldRemoveAlbum()
    {
        // Arrange
        var albumId = Guid.NewGuid();
        _mockRepository.Setup(r => r.DeleteAlbumAsync(albumId)).Returns(Task.CompletedTask);

        // Act
        await _albumService.DeleteAlbumAsync(albumId);

        // Assert
        _mockRepository.Verify(r => r.DeleteAlbumAsync(albumId), Times.Once);
    }

    [Fact]
    public async Task GetAlbumByIdAsync_ShouldReturnAlbumDto()
    {
        // Arrange
        var albumId = Guid.NewGuid();
        var album = new AlbumModel
        {
            Id = albumId,
            Name = "Test Album",
            Artist = "Test Artist",
            ImageUrl = "http://testimage.com",
            Tracks = new List<Track>() // voeg eventueel lege of test tracks toe als je wilt
        };

        // Voeg toe aan in-memory DbContext
        _context.Albums.Add(album);
        await _context.SaveChangesAsync();

        // Act
        var result = await _albumService.GetAlbumByIdAsync(albumId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(albumId, result.Id);
        Assert.Equal(album.Name, result.Name);
        Assert.Equal(album.Artist, result.Artist);
        Assert.Equal(album.ImageUrl, result.ImageUrl);
    }


    [Fact]
    public async Task GetAllAlbums_ShouldReturnAllAlbums()
    {
        // Arrange
        var albums = new List<AlbumModel>
    {
        new AlbumModel { Id = Guid.NewGuid(), Name = "Album1", Artist = "Artist1", ImageUrl = "http://image1.com" },
        new AlbumModel { Id = Guid.NewGuid(), Name = "Album2", Artist = "Artist2", ImageUrl = "http://image2.com" }
    };

        // Voeg albums toe aan de in-memory DbContext
        _context.Albums.AddRange(albums);
        await _context.SaveChangesAsync();

        // Act
        var result = await _albumService.GetAllAlbumsAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, a => a.Name == "Album1");
        Assert.Contains(result, a => a.Name == "Album2");
    }



    [Fact]
    public async Task UpdateAlbumAsync_ShouldUpdateAlbum()
    {
        // Arrange
        var albumId = Guid.NewGuid();
        var album = new AlbumModel
        {
            Id = albumId,
            Name = "Old Name",
            Artist = "Old Artist",
            ImageUrl = "http://oldimage.com"
        };

        var updateAlbumDto = new UpdateAlbumDto
        {
            Name = "New Name",
            Artist = "New Artist",
            ImageUrl = "http://newimage.com"
        };

        _mockRepository.Setup(r => r.GetAlbumByIdAsync(albumId)).ReturnsAsync(album);
        _mockRepository.Setup(r => r.UpdateAlbumAsync(It.Is<AlbumModel>(a =>
            a.Id == albumId &&
            a.Name == updateAlbumDto.Name &&
            a.Artist == updateAlbumDto.Artist &&
            a.ImageUrl == updateAlbumDto.ImageUrl))).Returns(Task.CompletedTask);

        // Act
        await _albumService.UpdateAlbumAsync(albumId, updateAlbumDto);

        // Assert
        Assert.Equal(updateAlbumDto.Name, album.Name);
        Assert.Equal(updateAlbumDto.Artist, album.Artist);
        Assert.Equal(updateAlbumDto.ImageUrl, album.ImageUrl);

        _mockRepository.Verify(r => r.UpdateAlbumAsync(It.Is<AlbumModel>(a =>
            a.Id == albumId &&
            a.Name == updateAlbumDto.Name &&
            a.Artist == updateAlbumDto.Artist &&
            a.ImageUrl == updateAlbumDto.ImageUrl)), Times.Once);
    }

    [Fact]
    public async Task UpdateAlbumAsync_ShouldThrowKeyNotFoundException_WhenAlbumNotFound()
    {
        // Arrange
        var albumId = Guid.NewGuid();
        var updateAlbumDto = new UpdateAlbumDto
        {
            Name = "New Name",
            Artist = "New Artist",
            ImageUrl = "http://newimage.com"
        };

        _mockRepository.Setup(r => r.GetAlbumByIdAsync(albumId)).ReturnsAsync((AlbumModel)null!);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _albumService.UpdateAlbumAsync(albumId, updateAlbumDto));
    }

    [Fact]
    public async Task UpdateAlbumAsync_ShouldThrowArgumentException_WhenAlbumAlreadyExists()
    {
        // Arrange
        var albumId = Guid.NewGuid();
        var existingAlbum = new AlbumModel
        {
            Id = albumId,
            Name = "Old Name",
            Artist = "Old Artist",
            ImageUrl = "http://oldimage.com"
        };

        var updateAlbumDto = new UpdateAlbumDto
        {
            Name = "New Name",
            Artist = "New Artist",
            ImageUrl = "http://newimage.com"
        };

        _mockRepository.Setup(r => r.GetAlbumByIdAsync(albumId)).ReturnsAsync(existingAlbum);
        _mockRepository.Setup(r => r.AlbumExistsByNameAndArtistAsync(updateAlbumDto.Name, updateAlbumDto.Artist)).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _albumService.UpdateAlbumAsync(albumId, updateAlbumDto));
    }
}