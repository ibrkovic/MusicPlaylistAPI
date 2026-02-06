namespace MusicPlaylistAPI.DTOs
{
    public class GenreDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        // Pjesme ovog žanra
        public ICollection<SongDTO> Songs { get; set; }
    }
}