using Album.Api.Data;
using Album.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Album.Api.Repositories;

public class AlbumRepository : IAlbumRepository
{
    private readonly AlbumContext _context;

    public AlbumRepository(AlbumContext context)
    {
        _context = context;
    }

    public async Task<Models.Album> CreateAlbumAsync(Models.Album album)
    {
        _context.Albums.Add(album);
        await _context.SaveChangesAsync();
        return album;
    }

    public async Task DeleteAlbumAsync(Guid id)
    {
        var album = await _context.Albums.FindAsync(id);
        if (album != null)
        {
            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Models.Album?> GetAlbumByIdAsync(Guid id)
    {
        return await _context.Albums.FindAsync(id);
    }

    public async Task<IEnumerable<Models.Album>> GetAllAlbumsAsync()
    {
        return await _context.Albums.ToListAsync();
    }

    public async Task UpdateAlbumAsync(Models.Album album)
    {
        _context.Entry(album).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<bool> AlbumExistsAsync(Guid id)
    {
        return await _context.Albums.AnyAsync(e => e.Id == id);
    }

    public async Task<bool> AlbumExistsByNameAndArtistAsync(string name, string artist)
    {
        return await _context.Albums.AnyAsync(a => a.Name == name && a.Artist == artist);
    }
}