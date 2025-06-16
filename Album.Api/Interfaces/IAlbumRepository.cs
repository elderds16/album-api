namespace Album.Api.Interfaces;

public interface IAlbumRepository
{
	Task<Models.Album> CreateAlbumAsync(Models.Album album);
	Task DeleteAlbumAsync(Guid id);
	Task<Models.Album?> GetAlbumByIdAsync(Guid id);
	Task<IEnumerable<Models.Album>> GetAllAlbumsAsync();
	Task UpdateAlbumAsync(Models.Album album);
	Task<bool> AlbumExistsAsync(Guid id);
	Task<bool> AlbumExistsByNameAndArtistAsync(string name, string artist);
}
