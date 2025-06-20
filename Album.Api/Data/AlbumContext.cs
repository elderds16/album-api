using Microsoft.EntityFrameworkCore;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Models.Album>()
            .HasIndex(a => new { a.Name, a.Artist })
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}

