namespace MusicPlaylistAPI.DTOs
{
    public class ArtistDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Biography { get; set; }

        // Pjesme izvođača
        public ICollection<SongDTO> Songs { get; set; }
    }
}