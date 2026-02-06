namespace MusicPlaylistAPI.DTOs
{
    public class SongDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; } // u sekundama
        public DateTime ReleaseDate { get; set; }

        // Izvođač
        public ArtistDTO Artist { get; set; }

        // Žanrovi
        public ICollection<GenreDTO> Genres { get; set; }
    }
}