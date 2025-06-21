namespace Album.Api.Models
{
    public class Track
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public int Duration { get; set; } // in seconds

        // Foreign key
        public Guid AlbumId { get; set; }  
        public Album Album { get; set; } = null!;

    }
}
