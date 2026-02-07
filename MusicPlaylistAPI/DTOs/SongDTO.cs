using System.ComponentModel.DataAnnotations;

namespace MusicPlaylistAPI.DTOs
{
    public class SongDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        [Range(1, 3600)]
        public int Duration { get; set; } // u sekundama

        public DateTime ReleaseDate { get; set; }

        public ArtistDTO? Artist { get; set; }

        public ICollection<GenreDTO>? Genres { get; set; }
    }

    public class CreateSongDTO
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        [Range(1, 3600)]
        public int Duration { get; set; }

        public DateTime ReleaseDate { get; set; }

        [Required]
        public int ArtistId { get; set; }

        public List<int>? GenreIds { get; set; }
    }

    public class UpdateSongDTO
    {
        [StringLength(100)]
        public string? Title { get; set; }

        [Range(1, 3600)]
        public int? Duration { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public int? ArtistId { get; set; }

        public List<int>? GenreIds { get; set; }
    }
}