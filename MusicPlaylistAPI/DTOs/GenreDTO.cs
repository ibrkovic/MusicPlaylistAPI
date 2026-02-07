using System.ComponentModel.DataAnnotations;

namespace MusicPlaylistAPI.DTOs
{
    public class GenreDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ICollection<SongDTO> Songs { get; set; } = new List<SongDTO>();
    }

    public class CreateGenreDTO
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Description { get; set; }
    }

    public class UpdateGenreDTO
    {
        [StringLength(50)]
        public string? Name { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }
    }
}