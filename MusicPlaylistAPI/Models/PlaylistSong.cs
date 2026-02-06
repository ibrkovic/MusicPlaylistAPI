using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlaylistAPI.Models
{
    public class PlaylistSong
    {
        public int PlaylistId { get; set; }
        [ForeignKey("PlaylistId")]
        public Playlist Playlist { get; set; } = null!;

        public int SongId { get; set; }
        [ForeignKey("SongId")]
        public Song Song { get; set; } = null!;

        public int Position { get; set; } // Pozicija u playlisti
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}