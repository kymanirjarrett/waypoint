using Microsoft.EntityFrameworkCore;
using Waypoint.Models;

namespace Waypoint.Data;

public class WaypointDbContext(DbContextOptions<WaypointDbContext> options) : DbContext(options)
{
    public DbSet<Destination> Destinations => Set<Destination>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Destination>(entity =>
        {
            entity.HasIndex(d => d.Slug).IsUnique();
            entity.HasData(SeedData.Destinations);
        });
    }
}
