using Album.Api.Interfaces;
using Album.Api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Album.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
public class AlbumController : ControllerBase
{
    private readonly IAlbumService _albumService;

    public AlbumController(IAlbumService albumService)
    {
        _albumService = albumService;
    }

    /// <summary>
    /// Haalt alle albums op
    /// </summary>
    /// <returns>Lijst van albums</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AlbumDto>), 200)]
    public async Task<ActionResult<IEnumerable<AlbumDto>>> GetAlbums()
    {
        var albums = await _albumService.GetAllAlbumsAsync();
        return Ok(albums);
    }

    /// <summary>
    /// Haalt een specifiek album op op basis van ID
    /// </summary>
    /// <param name="id">Album ID (GUID)</param>
    /// <returns>Gevonden album</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AlbumDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<AlbumDto>> GetAlbum(Guid id)
    {
        var album = await _albumService.GetAlbumByIdAsync(id);
        if (album == null)
        {
            return NotFound();
        }

        return album;
    }

    /// <summary>
    /// Wijzigt een bestaand album
    /// </summary>
    /// <param name="id">Album ID</param>
    /// <param name="updateAlbumDto">Geüpdatete gegevens</param>
    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> PutAlbum(Guid id, UpdateAlbumDto updateAlbumDto)
    {
        try
        {
            await _albumService.UpdateAlbumAsync(id, updateAlbumDto);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (await _albumService.GetAlbumByIdAsync(id) == null)
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    /// <summary>
    /// Maakt een nieuw album aan
    /// </summary>
    /// <param name="createAlbumDto">Albumgegevens</param>
    /// <returns>Gemaakt album</returns>
    [HttpPost]
    [ProducesResponseType(typeof(AlbumDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<AlbumDto>> PostAlbum(CreateAlbumDto createAlbumDto)
    {
        AlbumDto createdAlbum;
        try
        {
            createdAlbum = await _albumService.CreateAlbumAsync(createAlbumDto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        return CreatedAtAction(nameof(GetAlbum), new { id = createdAlbum.Id }, createdAlbum);
    }

    /// <summary>
    /// Verwijdert een album op basis van ID
    /// </summary>
    /// <param name="id">Album ID</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteAlbum(Guid id)
    {
        var album = await _albumService.GetAlbumByIdAsync(id);
        if (album == null)
        {
            return NotFound();
        }

        await _albumService.DeleteAlbumAsync(id);
        return NoContent();
    }
}
