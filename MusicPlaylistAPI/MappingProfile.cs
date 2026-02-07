using AutoMapper;
using MusicPlaylistAPI.DTOs;
using MusicPlaylistAPI.Models;

namespace MusicPlaylistAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ========== USER MAPPINGS ==========
            CreateMap<User, UserDTO>();
            CreateMap<CreateUserDTO, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<UpdateUserDTO, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // ========== ARTIST MAPPINGS ==========
            CreateMap<Artist, ArtistDTO>();
            CreateMap<CreateArtistDTO, Artist>();
            CreateMap<UpdateArtistDTO, Artist>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // ========== SONG MAPPINGS ==========
            CreateMap<Song, SongDTO>()
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.DurationInSeconds))
                .ForMember(dest => dest.Genres,
                    opt => opt.MapFrom(src => src.SongGenres.Select(sg => sg.Genre)))
                .ForMember(dest => dest.Artist,
                    opt => opt.MapFrom(src => src.Artist));

            CreateMap<CreateSongDTO, Song>()
                .ForMember(dest => dest.DurationInSeconds, opt => opt.MapFrom(src => src.Duration))
                .ForMember(dest => dest.SongGenres, opt => opt.Ignore())
                .ForMember(dest => dest.Artist, opt => opt.Ignore());

            CreateMap<UpdateSongDTO, Song>()
                .ForMember(dest => dest.DurationInSeconds, opt => opt.MapFrom(src => src.Duration ?? 0))
                .ForMember(dest => dest.SongGenres, opt => opt.Ignore())
                .ForMember(dest => dest.Artist, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // ========== GENRE MAPPINGS ==========
            CreateMap<Genre, GenreDTO>();
            CreateMap<CreateGenreDTO, Genre>();
            CreateMap<UpdateGenreDTO, Genre>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // ========== PLAYLIST MAPPINGS ==========
            CreateMap<Playlist, PlaylistDTO>()
                .ForMember(dest => dest.Owner,
                    opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Songs,
                    opt => opt.MapFrom(src => src.PlaylistSongs.Select(ps => ps.Song)));

            CreateMap<CreatePlaylistDTO, Playlist>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.PlaylistSongs, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<UpdatePlaylistDTO, Playlist>()
                .ForMember(dest => dest.PlaylistSongs, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // ========== REVERSE MAPPINGS ==========
            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Playlists, opt => opt.Ignore());

            CreateMap<PlaylistDTO, Playlist>()
                .ForMember(dest => dest.PlaylistSongs, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}