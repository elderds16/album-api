using Album.Api.Interfaces;
using Album.Api.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using AlbumModel = Album.Api.Models.Album;
using Album.Api.Data;



namespace Album.Api.Services;

public class AlbumService : IAlbumService
{
    private readonly IAlbumRepository _albumRepository;
    private readonly AlbumContext _context;



    public AlbumService(IAlbumRepository albumRepository, AlbumContext context)
    {
        _albumRepository = albumRepository;
        _context = context;
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
            ImageUrl = album.ImageUrl,
            Tracks = album.Tracks?.Select(t => new TrackDto
            {
                Id = t.Id,
                Title = t.Title,
                Artist = t.Artist,
                Duration = t.Duration
            }).ToList() ?? new List<TrackDto>()
        };


    }

    public async Task DeleteAlbumAsync(Guid id)
    {
        await _albumRepository.DeleteAlbumAsync(id);
    }

    public async Task<AlbumDto?> GetAlbumByIdAsync(Guid id)
    {
        // Laad album met tracks (gebruik context of repository)
        var album = await _context.Albums
            .Include(a => a.Tracks)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (album == null)
        {
            return null;
        }

        return new AlbumDto
        {
            Id = album.Id,
            Name = album.Name,
            Artist = album.Artist,
            ImageUrl = album.ImageUrl,
            Tracks = album.Tracks.Select(t => new TrackDto
            {
                Id = t.Id,
                Title = t.Title,
                Artist = t.Artist,
                Duration = t.Duration
            }).ToList()
        };
    }


    public async Task<IEnumerable<AlbumDto>> GetAllAlbumsAsync()
    {
        var albums = await _context.Albums
            .Include(a => a.Tracks)
            .ToListAsync();

        return albums.Select(a => new AlbumDto
        {
            Id = a.Id,
            Name = a.Name,
            Artist = a.Artist,
            ImageUrl = a.ImageUrl,
            Tracks = a.Tracks.Select(t => new TrackDto
            {
                Id = t.Id,
                Title = t.Title,
                Artist = t.Artist,
                Duration = t.Duration
            }).ToList()
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

