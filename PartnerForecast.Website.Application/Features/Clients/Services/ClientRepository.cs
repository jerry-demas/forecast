using Cbiz.SharedPackages;
using Microsoft.EntityFrameworkCore;
using PartnerForecast.Website.Application.Features.Clients.Contracts;
using PartnerForecast.Website.Application.Features.Clients.Models;
using PartnerForecast.Website.Application.Shared;
using PartnerForecast.Website.Application.Shared.Data;
using System.Linq.Expressions;

namespace PartnerForecast.Website.Application.Features.Clients.Services;

public class ClientRepository(ClientDataContext clientDataContext
) : IClientRepository
{
    private readonly ClientDataContext _dataContext = clientDataContext;

    public Task<Either<IEnumerable<Client>, PartnerForecastException>> GetClients(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Either<IEnumerable<Client>, PartnerForecastException>> GetClientsContains(string value, CancellationToken cancellationToken)
        => await GetClientsInternalAsync(
            c => c.ClientName.Contains(value),
            cancellationToken);




    private async Task <Either<IEnumerable<Client>, PartnerForecastException>> GetClientsInternalAsync(
        Expression<Func<ClientTable, bool>>? predicate,
        CancellationToken cancellationToken
    )
    {
        try
        {
            IQueryable<ClientTable> query = _dataContext.Set<ClientTable>();
            query = query.Where(c => c.ClientStatus == ForecastConstants.CLIENT_STATUS_ACTIVE);
            if (predicate is not null)
            {
                query = query.Where(predicate);               
            }
            return await query
                .AsNoTracking()
                .OrderBy(c => c.ClientName)
                .Select(c => new Client(
                    c.ClientNumber,
                    c.ClientName
                ))
                .ToListAsync(cancellationToken);                            
        }
        catch (Exception ex)
        {
            return new PartnerForecastException($"Error retrieving clients: {ex.Message}");
        }
    }



}
