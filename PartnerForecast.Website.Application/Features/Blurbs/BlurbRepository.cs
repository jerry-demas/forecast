using Cbiz.SharedPackages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace PartnerForecast.Website.Application.Features.Blurbs;

public class BlurbRepository
{
    private readonly ILogger<BlurbRepository> _logger;
    private readonly BlurbDbContext _dbContext;

    public BlurbRepository(ILogger<BlurbRepository> logger, BlurbDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public Either<Guid, Exception> CreateBlurb(Blurb blurb)
    {
        _logger.LogInformation("Saving blurb for {UserId}", blurb.CreatedBy);

        try
        {
            _dbContext.Add(blurb);
            _dbContext.SaveChanges();

            // NOTE: This would normally not be needed as if context was scoped it would be disposed of after each request
            _dbContext.ChangeTracker.Clear();
        }
        catch (Exception ex)
        {
            return new BlurbSqlException(ex);
        }

        return blurb.BlurbId;
    }

    public Possible<Exception> DeleteBlurb(Guid blurbId)
    {
        Blurb? blurb = _dbContext.Blurbs.FirstOrDefault(x => x.BlurbId == blurbId);

        if (blurb is null)
        {
            _logger.LogWarning("Cannot find Blurb {BlurbId}", blurbId);
            return new BlurbNotFoundException(blurbId);
        }

        try 
        { 
            blurb.IsActive = false;
            int records = _dbContext.SaveChanges();
        
            // NOTE: This would normally not be needed as if context was scoped it would be disposed of after each request
            _dbContext.ChangeTracker.Clear();

            if (records != 1)
            {
                return new BlurbSqlException($"Blurb deleted, but {records} rows affected, but only 1 expected.");
            }
            else
            {
                return Possible.Completed;
            }                
        }
        catch (Exception ex)
        {
            return new BlurbSqlException(ex);
        }
    }

    public Either<List<Blurb>, Exception> GetBlurbs(Func<Blurb, bool>? filter)
    {
        filter = filter ?? (x => true);
        try
        {
            return _dbContext.Blurbs.AsNoTracking().Where(filter).ToList();
        }
        catch (Exception ex)
        {
            return new BlurbSqlException(ex);
        }
    }

    public Either<Blurb, Exception> GetBlurb(Guid blurbId)
    {
        Either<Blurb, Exception> retVal = new Exception("not run");

        GetBlurbs(x => x.BlurbId == blurbId).Match(
            success =>
            {
                if (success is null || success.Count == 0)
                {
                    retVal = new BlurbNotFoundException(blurbId);
                }
                else
                {
                    retVal = success.First();
                }
            },
            forFailure :(_, ex) => { retVal = ex; }
            );

        return retVal;
    }

    public Possible<Exception> UpdateBlurb(Blurb blurb)
    {
        _logger.LogInformation("Updating blurb {BlurbId}", blurb.BlurbId);
        try
        {
            Blurb? exstingBlurb = _dbContext.Blurbs.FirstOrDefault(x => x.BlurbId == blurb.BlurbId);

            if (exstingBlurb is null)
            {
                _logger.LogWarning("Cannot find Blurb {BlurbId}", blurb.BlurbId);
                return new BlurbNotFoundException(blurb.BlurbId);
            }

            exstingBlurb.Text = blurb.Text;
            exstingBlurb.IsPrivate = blurb.IsPrivate;
            exstingBlurb.ModifiedUtcDate = DateTime.UtcNow;

            int records = _dbContext.SaveChanges();

            // NOTE: This would normally not be needed as if context was scoped it would be disposed of after each request
            _dbContext.ChangeTracker.Clear();

            if (records != 1)
            {
                return new BlurbSqlException($"Blurb deleted, but {records} rows affected, but only 1 expected.");
            }
            else
            {
                return Possible.Completed;
            }
        }
        catch (Exception ex)
        {
            return new BlurbSqlException(ex);
        }
    }
}