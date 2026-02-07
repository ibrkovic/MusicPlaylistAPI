using System.ComponentModel.DataAnnotations;

namespace MusicPlaylistAPI.DTOs
{
    public class ArtistDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Biography { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public ICollection<SongDTO> Songs { get; set; } = new List<SongDTO>();
    }

    public class CreateArtistDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Biography { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }

    public class UpdateArtistDTO
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(500)]
        public string? Biography { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }
}