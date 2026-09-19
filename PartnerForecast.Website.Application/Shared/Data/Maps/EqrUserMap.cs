using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PartnerForecast.Website.Application.Features.Users.Models;

namespace PartnerForecast.Website.Application.Shared.Data.Maps;

internal class NaoUserMap : IEntityTypeConfiguration<EqrUser>
{
 public void Configure(EntityTypeBuilder<EqrUser> builder)
 {
     builder.ToTable("EQRUsers").HasKey(_ => _.Id);     
 }
}
