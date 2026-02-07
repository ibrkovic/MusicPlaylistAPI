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
    public class PlaylistsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PlaylistsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Playlists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaylistDTO>>> GetPlaylists()
        {
            var playlists = await _context.Playlists
                .Include(p => p.User)
                .Include(p => p.PlaylistSongs)
                    .ThenInclude(ps => ps.Song)
                        .ThenInclude(s => s.Artist)
                .ToListAsync();

            return Ok(_mapper.Map<List<PlaylistDTO>>(playlists));
        }

        // GET: api/Playlists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlaylistDTO>> GetPlaylist(int id)
        {
            var playlist = await _context.Playlists
                .Include(p => p.User)
                .Include(p => p.PlaylistSongs)
                    .ThenInclude(ps => ps.Song)
                        .ThenInclude(s => s.Artist)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (playlist == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PlaylistDTO>(playlist));
        }

        // POST: api/Playlists
        [HttpPost]
        public async Task<ActionResult<PlaylistDTO>> PostPlaylist(CreatePlaylistDTO createPlaylistDto)
        {
            // Provjeri da li korisnik postoji
            var user = await _context.Users.FindAsync(createPlaylistDto.UserId);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var playlist = _mapper.Map<Playlist>(createPlaylistDto);

            // Dodaj pjesme u playlistu ako postoje
            if (createPlaylistDto.SongIds != null && createPlaylistDto.SongIds.Any())
            {
                var position = 1;
                foreach (var songId in createPlaylistDto.SongIds)
                {
                    var song = await _context.Songs.FindAsync(songId);
                    if (song != null)
                    {
                        playlist.PlaylistSongs.Add(new PlaylistSong
                        {
                            SongId = songId,
                            Position = position++,
                            AddedAt = DateTime.UtcNow
                        });
                    }
                }
            }

            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();

            // Dohvati kreiranu playlistu sa svim podacima
            var createdPlaylist = await _context.Playlists
                .Include(p => p.User)
                .Include(p => p.PlaylistSongs)
                    .ThenInclude(ps => ps.Song)
                .FirstOrDefaultAsync(p => p.Id == playlist.Id);

            return CreatedAtAction("GetPlaylist",
                new { id = playlist.Id },
                _mapper.Map<PlaylistDTO>(createdPlaylist));
        }

        // PUT: api/Playlists/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlaylist(int id, UpdatePlaylistDTO updatePlaylistDto)
        {
            var playlist = await _context.Playlists
                .Include(p => p.PlaylistSongs)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (playlist == null)
            {
                return NotFound();
            }

            // Ažuriraj osnovne podatke
            if (!string.IsNullOrEmpty(updatePlaylistDto.Name))
                playlist.Name = updatePlaylistDto.Name;

            if (!string.IsNullOrEmpty(updatePlaylistDto.Description))
                playlist.Description = updatePlaylistDto.Description;

            if (updatePlaylistDto.IsPublic.HasValue)
                playlist.IsPublic = updatePlaylistDto.IsPublic.Value;

            // Ažuriraj pjesme ako su dostavljene
            if (updatePlaylistDto.SongIds != null)
            {
                playlist.PlaylistSongs.Clear();

                var position = 1;
                foreach (var songId in updatePlaylistDto.SongIds)
                {
                    var song = await _context.Songs.FindAsync(songId);
                    if (song != null)
                    {
                        playlist.PlaylistSongs.Add(new PlaylistSong
                        {
                            PlaylistId = playlist.Id,
                            SongId = songId,
                            Position = position++,
                            AddedAt = DateTime.UtcNow
                        });
                    }
                }
            }

            _context.Entry(playlist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaylistExists(id))
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

        // DELETE: api/Playlists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaylist(int id)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist == null)
            {
                return NotFound();
            }

            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlaylistExists(int id)
        {
            return _context.Playlists.Any(e => e.Id == id);
        }
    }
}