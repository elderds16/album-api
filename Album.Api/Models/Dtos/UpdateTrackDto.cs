namespace Album.Api.Models.Dtos
{
    public class UpdateTrackDto
    {
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public int Duration { get; set; }
    }
}