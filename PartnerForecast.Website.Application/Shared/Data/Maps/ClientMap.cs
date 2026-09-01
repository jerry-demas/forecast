
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PartnerForecast.Website.Application.Features.Clients.Models;


namespace PartnerForecast.Website.Application.Shared.Data.Maps;
    
internal class ClientTableMap : IEntityTypeConfiguration<ClientTable>
{
    
    public void Configure(EntityTypeBuilder<ClientTable> builder)
    {        
        builder.ToView("vwIntegrations_Client_1.0","Client").HasNoKey();
        builder.Property(c => c.ClientNumber).HasColumnName("Client_Code");
        builder.Property(c => c.ClientName).HasColumnName("Client_Name");
        builder.Property(c => c.ClientStatus).HasColumnName("Client_Status");
    }   


}

