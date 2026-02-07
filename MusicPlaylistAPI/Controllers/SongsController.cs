using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicPlaylistAPI.Data;
using MusicPlaylistAPI.DTOs;
using MusicPlaylistAPI.Models;
using AutoMapper;

namespace MusicPlaylistAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SongsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Songs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SongDTO>>> GetSongs()
        {
            var songs = await _context.Songs
                .Include(s => s.Artist)
                .Include(s => s.SongGenres)
                    .ThenInclude(sg => sg.Genre)
                .ToListAsync();

            return Ok(_mapper.Map<List<SongDTO>>(songs));
        }

        // GET: api/Songs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SongDTO>> GetSong(int id)
        {
            var song = await _context.Songs
                .Include(s => s.Artist)
                .Include(s => s.SongGenres)
                    .ThenInclude(sg => sg.Genre)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (song == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<SongDTO>(song));
        }

        // POST: api/Songs
        [HttpPost]
        public async Task<ActionResult<SongDTO>> PostSong(CreateSongDTO createSongDto)
        {
            var song = _mapper.Map<Song>(createSongDto);

            // Dodaj žanrove ako postoje
            if (createSongDto.GenreIds != null && createSongDto.GenreIds.Any())
            {
                foreach (var genreId in createSongDto.GenreIds)
                {
                    var genre = await _context.Genres.FindAsync(genreId);
                    if (genre != null)
                    {
                        song.SongGenres.Add(new SongGenre
                        {
                            GenreId = genreId,
                            Song = song
                        });
                    }
                }
            }

            _context.Songs.Add(song);
            await _context.SaveChangesAsync();

            // Učitaj kreiranu pjesmu sa svim podacima
            var createdSong = await _context.Songs
                .Include(s => s.Artist)
                .Include(s => s.SongGenres)
                    .ThenInclude(sg => sg.Genre)
                .FirstOrDefaultAsync(s => s.Id == song.Id);

            return CreatedAtAction("GetSong", new { id = song.Id }, _mapper.Map<SongDTO>(createdSong));
        }

        // PUT: api/Songs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSong(int id, UpdateSongDTO updateSongDto)
        {
            var song = await _context.Songs
                .Include(s => s.SongGenres)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (song == null)
            {
                return NotFound();
            }

            // Update osnovne podatke
            if (!string.IsNullOrEmpty(updateSongDto.Title))
                song.Title = updateSongDto.Title;

            if (updateSongDto.Duration.HasValue)
                song.DurationInSeconds = updateSongDto.Duration.Value;

            if (updateSongDto.ReleaseDate.HasValue)
                song.ReleaseDate = updateSongDto.ReleaseDate.Value;

            if (updateSongDto.ArtistId.HasValue)
                song.ArtistId = updateSongDto.ArtistId.Value;

            // Update žanrove ako su dostavljeni
            if (updateSongDto.GenreIds != null)
            {
                // Ukloni postojeće žanrove
                song.SongGenres.Clear();

                // Dodaj nove žanrove
                foreach (var genreId in updateSongDto.GenreIds)
                {
                    var genre = await _context.Genres.FindAsync(genreId);
                    if (genre != null)
                    {
                        song.SongGenres.Add(new SongGenre
                        {
                            GenreId = genreId,
                            SongId = song.Id
                        });
                    }
                }
            }

            _context.Entry(song).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SongExists(id))
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

        // DELETE: api/Songs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSong(int id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song == null)
            {
                return NotFound();
            }

            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SongExists(int id)
        {
            return _context.Songs.Any(e => e.Id == id);
        }
    }
}