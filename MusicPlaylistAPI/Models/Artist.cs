using System.ComponentModel.DataAnnotations;

namespace MusicPlaylistAPI.Models
{
    public class Artist
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [StringLength(500)]
        public string? Biography { get; set; }

        public DateTime? DateOfBirth { get; set; }

        // 1:N odnos - jedan izvođač može imati više pjesama
        public ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}