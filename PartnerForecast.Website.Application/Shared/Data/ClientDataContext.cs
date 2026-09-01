using Microsoft.EntityFrameworkCore;
using PartnerForecast.Website.Application.Features.Clients.Models;


namespace PartnerForecast.Website.Application.Shared.Data;

public class ClientDataContext : DbContext
{
     public DbSet<ClientTable> Clients { get; set; }

    public ClientDataContext(DbContextOptions<ClientDataContext> options) : base(options)
    {
    }    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {        
        modelBuilder.ApplyConfiguration(new Maps.ClientTableMap());
    }

}