using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlaylistAPI.Models
{
    public class Song
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } =null!;

        [Required]
        [Range(1, 3600)]
        public int DurationInSeconds { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        // 1:N odnos - pjesma pripada jednom izvođaču
        public int ArtistId { get; set; }
        [ForeignKey("ArtistId")]
        public Artist Artist { get; set; } = null!;

        // N:M odnos - pjesma može pripadati više playlisti
        public ICollection<PlaylistSong> PlaylistSongs { get; set; } = new List<PlaylistSong>();

        // N:M odnos - pjesma može imati više žanrova
        public ICollection<SongGenre> SongGenres { get; set; } = new List<SongGenre>();
    }
}