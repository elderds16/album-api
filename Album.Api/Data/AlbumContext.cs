using Microsoft.EntityFrameworkCore;
using Album.Api.Models;

namespace Album.Api.Data;

public class AlbumContext : DbContext
{
    public AlbumContext(DbContextOptions<AlbumContext> options) : base(options)
    {
    }

    public AlbumContext()
    {
    }

    public virtual DbSet<Models.Album> Albums { get; set; } = null!;
    public virtual DbSet<Models.Track> Tracks { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Track>()
            .HasOne(t => t.Album)
            .WithMany(a => a.Tracks)
            .HasForeignKey(t => t.AlbumId)
            .OnDelete(DeleteBehavior.Cascade); // als album verwijderd wordt, verwijder ook tracks
    }

}