using Album.Api.Data;
using Album.Api.Models;
using Album.Api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Album.Api.Controllers
{
    [Route("api/albums/{albumId}/tracks")]
    [ApiController]
    public class TrackController : ControllerBase
    {
        private readonly AlbumContext _context;

        public TrackController(AlbumContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrack(Guid albumId, CreateTrackDto dto)
        {
            var album = await _context.Albums.FindAsync(albumId);
            if (album == null)
                return NotFound($"Album with ID {albumId} not found.");

            var track = new Track
            {
                Title = dto.Title,
                Artist = dto.Artist,
                Duration = dto.Duration,
                AlbumId = albumId // uit URL gebruiken
            };

            _context.Tracks.Add(track);
            await _context.SaveChangesAsync();

            var resultDto = new TrackDto
            {
                Id = track.Id,
                Title = track.Title,
                Artist = track.Artist,
                Duration = track.Duration
            };

            return CreatedAtAction(nameof(GetTrackById), new { albumId = albumId, id = track.Id }, resultDto);
        }


        /// <summary>
        /// Haalt een specifieke track op aan de hand van het ID.
        /// </summary>
        /// <param name="id">Het ID van de track.</param>
        /// <returns>De trackgegevens als deze bestaat, anders 404.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TrackDto>> GetTrackById(int id)
        {
            var track = await _context.Tracks.FindAsync(id);
            if (track == null)
                return NotFound();

            return new TrackDto
            {
                Id = track.Id,
                Title = track.Title,
                Artist = track.Artist,
                Duration = track.Duration
            };
        }

        /// <summary>
        /// Wijzigt een bestaande track.
        /// </summary>
        /// <param name="id">Het ID van de te wijzigen track.</param>
        /// <param name="dto">De nieuwe gegevens van de track.</param>
        /// <returns>Statuscode 204 bij succes, of 404 als de track niet bestaat.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrack(int id, UpdateTrackDto dto)
        {
            var track = await _context.Tracks.FindAsync(id);
            if (track == null)
                return NotFound();

            track.Title = dto.Title;
            track.Artist = dto.Artist;
            track.Duration = dto.Duration;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Verwijdert een track op basis van ID.
        /// </summary>
        /// <param name="id">Het ID van de te verwijderen track.</param>
        /// <returns>Statuscode 204 bij succes, of 404 als de track niet bestaat.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrack(int id)
        {
            var track = await _context.Tracks.FindAsync(id);
            if (track == null)
                return NotFound();

            _context.Tracks.Remove(track);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
