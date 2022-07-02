namespace MusicHub.Data
{
    using Microsoft.EntityFrameworkCore;
    using MusicHub.Data.Models;

    public class MusicHubDbContext : DbContext
    {
        public MusicHubDbContext()
        {
        }

        public MusicHubDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }
        public DbSet<Album> Albums { get; set; }

        public DbSet<Performer> Performers { get; set; }

        public DbSet<Producer> Producers { get; set; }

        public DbSet<Song> Songs { get; set; }
        public DbSet<SongPerformer> SongsPerformers { get; set; }

        public DbSet<Writer> Writers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SongPerformer>()
               .HasKey(x => new { x.SongId, x.PerformerId });
        }
    }
}
