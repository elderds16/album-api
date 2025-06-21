namespace Album.Api.Models.Dtos;

public class UpdateAlbumDto
{
    public string Name { get; set; } = null!;
    public string Artist { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
}
