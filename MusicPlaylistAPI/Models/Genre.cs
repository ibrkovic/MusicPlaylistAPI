using System.ComponentModel.DataAnnotations;

namespace MusicPlaylistAPI.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [StringLength(200)]
        public string? Description { get; set; }

        // N:M odnos - jedan žanr može pripadati više pjesama
        public ICollection<SongGenre> SongGenres { get; set; } = new List<SongGenre>();
    }
}