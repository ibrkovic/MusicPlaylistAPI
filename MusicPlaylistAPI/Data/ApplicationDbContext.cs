using Microsoft.EntityFrameworkCore;
using MusicPlaylistAPI.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace MusicPlaylistAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet za svaki entitet
        public DbSet<User> Users { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistSong> PlaylistSongs { get; set; }
        public DbSet<SongGenre> SongGenres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfiguracija N:M veze Playlist ↔ Song
            modelBuilder.Entity<PlaylistSong>()
                .HasKey(ps => new { ps.PlaylistId, ps.SongId });

            modelBuilder.Entity<PlaylistSong>()
                .HasOne(ps => ps.Playlist)
                .WithMany(p => p.PlaylistSongs)
                .HasForeignKey(ps => ps.PlaylistId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlaylistSong>()
                .HasOne(ps => ps.Song)
                .WithMany(s => s.PlaylistSongs)
                .HasForeignKey(ps => ps.SongId)
                .OnDelete(DeleteBehavior.Cascade);

            // Konfiguracija N:M veze Song ↔ Genre
            modelBuilder.Entity<SongGenre>()
                .HasKey(sg => new { sg.SongId, sg.GenreId });

            modelBuilder.Entity<SongGenre>()
                .HasOne(sg => sg.Song)
                .WithMany(s => s.SongGenres)
                .HasForeignKey(sg => sg.SongId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SongGenre>()
                .HasOne(sg => sg.Genre)
                .WithMany(g => g.SongGenres)
                .HasForeignKey(sg => sg.GenreId)
                .OnDelete(DeleteBehavior.Cascade);

            // Dodavanje početnih podataka (Seed data)
            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Pop", Description = "Popularna glazba" },
                new Genre { Id = 2, Name = "Rock", Description = "Rock glazba" },
                new Genre { Id = 3, Name = "Hip Hop", Description = "Hip Hop glazba" },
                new Genre { Id = 4, Name = "Jazz", Description = "Jazz glazba" },
                new Genre { Id = 5, Name = "Classical", Description = "Klasična glazba" }
            );

            modelBuilder.Entity<Artist>().HasData(
                new Artist { Id = 1, Name = "The Beatles", Biography = "Engleski rock sastav" },
                new Artist { Id = 2, Name = "Taylor Swift", Biography = "Američka pjevačica" },
                new Artist { Id = 3, Name = "Drake", Biography = "Kanadski reper" }
            );
        }
    }
}