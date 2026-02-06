using System.ComponentModel.DataAnnotations;

namespace MusicPlaylistAPI.DTOs
{
    public class PlaylistDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        // Vlasnik playliste
        public UserDTO Owner { get; set; }

        // Pjesme u playlisti
        public ICollection<SongDTO> Songs { get; set; }
    }

    public class CreatePlaylistDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public int UserId { get; set; }

        // Opcionalno: liste ID-eva pjesama
        public List<int>? SongIds { get; set; }
    }
}