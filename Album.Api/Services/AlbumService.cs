using Album.Api.Interfaces;
using Album.Api.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using AlbumModel = Album.Api.Models.Album;

namespace Album.Api.Services;

public class AlbumService : IAlbumService
{
	private readonly IAlbumRepository _albumRepository;

	public AlbumService(IAlbumRepository albumRepository)
	{
		_albumRepository = albumRepository;
	}

	public async Task<AlbumDto> CreateAlbumAsync(CreateAlbumDto createAlbumDto)
	{
		var albumExists = await _albumRepository.AlbumExistsByNameAndArtistAsync(createAlbumDto.Name, createAlbumDto.Artist);
		if (albumExists)
		{
			throw new ArgumentException("An album with the same name and artist already exists.");
		}

		var album = new AlbumModel
		{
			Id = Guid.NewGuid(),
			Name = createAlbumDto.Name,
			Artist = createAlbumDto.Artist,
			ImageUrl = createAlbumDto.ImageUrl
		};

		album = await _albumRepository.CreateAlbumAsync(album);

		return new AlbumDto
		{
			Id = album.Id,
			Name = album.Name,
			Artist = album.Artist,
			ImageUrl = album.ImageUrl
		};
	}

	public async Task DeleteAlbumAsync(Guid id)
	{
		await _albumRepository.DeleteAlbumAsync(id);
	}

	public async Task<AlbumDto?> GetAlbumByIdAsync(Guid id)
	{
		var album = await _albumRepository.GetAlbumByIdAsync(id);
		if (album == null)
		{
			return null;
		}

		return new AlbumDto
		{
			Id = album.Id,
			Name = album.Name,
			Artist = album.Artist,
			ImageUrl = album.ImageUrl
		};
	}

	public async Task<IEnumerable<AlbumDto>> GetAllAlbumsAsync()
	{
		var albums = await _albumRepository.GetAllAlbumsAsync();
		return albums.Select(a => new AlbumDto
		{
			Id = a.Id,
			Name = a.Name,
			Artist = a.Artist,
			ImageUrl = a.ImageUrl
		});
	}

	public async Task UpdateAlbumAsync(Guid id, UpdateAlbumDto updateAlbumDto)
	{
		var album = await _albumRepository.GetAlbumByIdAsync(id);
		if (album == null)
		{
			throw new KeyNotFoundException("Album not found");
		}

		if (album.Name != updateAlbumDto.Name || album.Artist != updateAlbumDto.Artist)
		{

			var albumExists = await _albumRepository.AlbumExistsByNameAndArtistAsync(updateAlbumDto.Name, updateAlbumDto.Artist);
			if (albumExists)
			{
				throw new ArgumentException("An album with the same name and artist already exists.");
			}
		}

		album.Name = updateAlbumDto.Name;
		album.Artist = updateAlbumDto.Artist;
		album.ImageUrl = updateAlbumDto.ImageUrl;

		try
		{
			await _albumRepository.UpdateAlbumAsync(album);
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!await _albumRepository.AlbumExistsAsync(id))
			{
				throw new KeyNotFoundException("Album not found");
			}
			throw;
		}
	}
}