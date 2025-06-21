namespace Album.Api.Models.Dtos
{
	public class CreateTrackDto
	{
		public string Title { get; set; } = string.Empty;
		public string Artist { get; set; } = string.Empty;
		public int Duration { get; set; }
		public Guid AlbumId { get; set; }
	}
}
