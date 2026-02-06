using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlaylistAPI.Models
{
    public class SongGenre
    {
        public int SongId { get; set; }
        [ForeignKey("SongId")]
        public Song Song { get; set; }

        public int GenreId { get; set; }
        [ForeignKey("GenreId")]
        public Genre Genre { get; set; }
    }
}