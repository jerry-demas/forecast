
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PartnerForecast.Website.Application.Features.AuditLogs.Models;

namespace PartnerForecast.Website.Application.Shared.Data.Maps;

internal class AuditLogMap  : IEntityTypeConfiguration<AuditLogTable>
{
    public void Configure(EntityTypeBuilder<AuditLogTable> builder)
    {

        builder.ToTable("AuditLog");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();
        builder.Property(p => p.LogCategory).IsRequired();
        builder.Property(p => p.ChangedByEmployeeNumber).IsRequired();
        builder.Property(p => p.ChangedByEmployeeName).IsRequired();
        builder.Property(p => p.ChangedByEmployeeDomain).IsRequired();
        builder.Property(p => p.ChangeDescription).IsRequired();
        builder.Property(p => p.CreatedDateTime).IsRequired();

    }

}
