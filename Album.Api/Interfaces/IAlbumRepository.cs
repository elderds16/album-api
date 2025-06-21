using Album.Api.Models.Dtos;

namespace Album.Api.Interfaces;

public interface IAlbumService
{
    Task<AlbumDto> CreateAlbumAsync(CreateAlbumDto createAlbumDto);
    Task DeleteAlbumAsync(Guid id);
    Task<AlbumDto?> GetAlbumByIdAsync(Guid id);
    Task<IEnumerable<AlbumDto>> GetAllAlbumsAsync();
    Task UpdateAlbumAsync(Guid id, UpdateAlbumDto updateAlbumDto);
}
