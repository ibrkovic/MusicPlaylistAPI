using System.ComponentModel.DataAnnotations;

namespace MusicPlaylistAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 1:N odnos - jedan korisnik može imati više playlisti
        public ICollection<Playlist> Playlists { get; set; }
    }
}