using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PartnerForecast.Website.Application.Features.TaskCodes.Modules;

namespace PartnerForecast.Website.Application.Shared.Data.Maps;

internal class TaskCodeMap : IEntityTypeConfiguration<TaskCode>
{
    public void Configure(EntityTypeBuilder<TaskCode> builder)
    {
        
        builder.ToTable("TaskCodes").HasKey(p => p.Id);        
        builder.Property(p => p.Id).ValueGeneratedOnAdd();
        builder.Property(p => p.Code).HasColumnName("TaskCode").IsRequired();
        builder.Property(p => p.CodeDescription).HasColumnName("TaskCodeDescription").IsRequired();
    }
}
