using Microsoft.EntityFrameworkCore;
using PartnerForecast.Website.Application.Features.AuditLogs.Models;
using PartnerForecast.Website.Application.Features.Hours.Models;
using PartnerForecast.Website.Application.Features.TaskCodes.Modules;
using PartnerForecast.Website.Application.Features.Users.Models;

namespace PartnerForecast.Website.Application.Shared.Data;

public class PartnerForecastDataContext : DbContext
{

    public DbSet<EqrUser> EqrUsers { get; set; }
    public DbSet<TaskCode> TaskCodes { get; set; }
    public DbSet<AuditLogTable> AuditLogs { get; set; }
    public DbSet<ClientHours> ClientHours { get; set; }
    
    public PartnerForecastDataContext(DbContextOptions<PartnerForecastDataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {        
        modelBuilder.ApplyConfiguration(new Maps.NaoUserMap());
        modelBuilder.ApplyConfiguration(new Maps.TaskCodeMap());
        modelBuilder.ApplyConfiguration(new Maps.AuditLogMap());
        modelBuilder.ApplyConfiguration(new Maps.ClientHoursMap());

        //modelBuilder.Entity<AuditLogTable>()
        //  .HasOne(al => al.ClientHours)
        //  .WithMany(ch => ch.AuditLogs)
         // .HasForeignKey(al => al.ClientHoursId)
        //  .IsRequired();
    }



}
