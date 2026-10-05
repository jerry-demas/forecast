using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PartnerForecast.Website.Application.Features.Hours.Models;

namespace PartnerForecast.Website.Application.Shared.Data.Maps;

internal class ClientHoursMap : IEntityTypeConfiguration<ClientHours>
{

    public void Configure(EntityTypeBuilder<ClientHours> builder)
    {
        builder.ToTable("ClientHours"); //, tb => tb.HasTrigger("trg_UpdateLastUpdated"));
        builder.HasKey(t => t.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();       
        builder.Property(p => p.EmployeeNumber).HasColumnName("employeeNumber").IsRequired();
        builder.Property(p => p.EmployeeNameAssigned).HasColumnName("EmployeeNameAssigned").IsRequired();
        builder.Property(p => p.EmployeeDomainAssigned).HasColumnName("EmployeeDomainAssigned").IsRequired();
        builder.Property(p => p.Month).HasColumnName("Month").IsRequired();
        builder.Property(p => p.Year).HasColumnName("Year").IsRequired();
        builder.Property(p => p.Hours).HasColumnName("Hours").IsRequired();
        builder.Property(p => p.IsDeleted).HasColumnName("isDeleted").IsRequired();
        builder.Property(p => p.CreatedDateTime).HasColumnName("CreatedDateTime").IsRequired();
        builder.Property(p => p.LastUpdatedDateTime).HasColumnName("LastUpdatedDateTime").IsRequired();
        builder.Property(p => p.IsEQR).HasColumnName("isEQR").IsRequired().HasDefaultValue(false);
        builder.Property(p => p.IsNonBillable).HasColumnName("isNonBillable").IsRequired().HasDefaultValue(false);
        builder.Property(p => p.EmployeeNumberAssigned).HasColumnName("EmployeeNumberAssigned").IsRequired().HasDefaultValue(false);

    }
}
