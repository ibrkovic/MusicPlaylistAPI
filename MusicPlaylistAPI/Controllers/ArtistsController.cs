using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicPlaylistAPI.Data;
using MusicPlaylistAPI.DTOs;
using MusicPlaylistAPI.Models;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace MusicPlaylistAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ArtistsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Artists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtistDTO>>> GetArtists()
        {
            var artists = await _context.Artists
                .Include(a => a.Songs)
                .ToListAsync();
            return Ok(_mapper.Map<List<ArtistDTO>>(artists));
        }

        // GET: api/Artists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArtistDTO>> GetArtist(int id)
        {
            var artist = await _context.Artists
                .Include(a => a.Songs)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (artist == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ArtistDTO>(artist));
        }

        // POST: api/Artists - KREIRANJE NOVOG IZVOĐAČA
        [HttpPost]
        public async Task<ActionResult<ArtistDTO>> PostArtist(CreateArtistDTO createArtistDto)
        {
            // Validacija
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Provjera da li već postoji izvođač sa istim imenom
            if (await _context.Artists.AnyAsync(a => a.Name == createArtistDto.Name))
            {
                return BadRequest(new { message = "Artist with this name already exists." });
            }

            // Mapiraj DTO na model
            var artist = _mapper.Map<Artist>(createArtistDto);

            // Dodaj u bazu
            _context.Artists.Add(artist);
            await _context.SaveChangesAsync();

            // Dohvati kreiranog izvođača sa svim podacima
            var createdArtist = await _context.Artists
                .Include(a => a.Songs)
                .FirstOrDefaultAsync(a => a.Id == artist.Id);

            // Vrati kreirani resurs sa lokacijom
            return CreatedAtAction(
                nameof(GetArtist),
                new { id = artist.Id },
                _mapper.Map<ArtistDTO>(createdArtist));
        }

        // PUT: api/Artists/5 - AŽURIRANJE POSTOJEĆEG IZVOĐAČA
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtist(int id, UpdateArtistDTO updateArtistDto)
        {
            // Provjeri da li se ID-ovi poklapaju
            if (id != updateArtistDto.Id)
            {
                return BadRequest("ID in URL does not match ID in body.");
            }

            // Validacija
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Pronađi postojećeg izvođača
            var artist = await _context.Artists.FindAsync(id);
            if (artist == null)
            {
                return NotFound();
            }

            // Provjeri da li novo ime već postoji (ako se mijenja)
            if (!string.IsNullOrEmpty(updateArtistDto.Name) &&
                updateArtistDto.Name != artist.Name &&
                await _context.Artists.AnyAsync(a => a.Name == updateArtistDto.Name))
            {
                return BadRequest(new { message = "Artist with this name already exists." });
            }

            // Mapiraj promjene
            _mapper.Map(updateArtistDto, artist);

            // Označi kao modificiran
            _context.Entry(artist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtistExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Artists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtist(int id)
        {
            var artist = await _context.Artists.FindAsync(id);
            if (artist == null)
            {
                return NotFound();
            }

            _context.Artists.Remove(artist);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArtistExists(int id)
        {
            return _context.Artists.Any(e => e.Id == id);
        }
    }
}