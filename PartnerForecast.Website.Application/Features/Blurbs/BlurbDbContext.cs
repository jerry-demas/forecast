using Microsoft.EntityFrameworkCore;

namespace PartnerForecast.Website.Application.Features.Blurbs;

public class BlurbDbContext : DbContext
{
    public BlurbDbContext(DbContextOptions<BlurbDbContext> options)
       : base(options)
    {
        // NOTE: This is temp code to allow for in memory database to be created on startup and used (memory db only exsits while open connection)
        Database.OpenConnection();
        Database.EnsureCreated();
    }

    public DbSet<Blurb> Blurbs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BlurbEntityTypeConfiguration).Assembly);
    }
}
