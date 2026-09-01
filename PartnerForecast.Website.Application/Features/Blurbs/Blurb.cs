using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PartnerForecast.Website.Application.Features.Blurbs;

public class Blurb
{
    public Guid BlurbId { get; set; } = Guid.Empty; // NOTE: This is set to empty to allow for EF to generate the value, if this was set to a new Guid it would generate a new value on each instance which would cause issues with updates and deletes
    public string Text { get; set; } = string.Empty;
    public bool IsPrivate { get; set; } = true;
    public DateTime CreatedUtcDate { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? ModifiedUtcDate { get; set; } = null;
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// Entity Framework Core configuration for the Blurb entity.
/// </summary>
public class BlurbEntityTypeConfiguration : IEntityTypeConfiguration<Blurb>
{
    public void Configure(EntityTypeBuilder<Blurb> builder)
    {
        builder.ToTable("Blurbs");
        builder.HasKey(b => b.BlurbId);
        builder.HasQueryFilter(b => b.IsActive);
    }
}

