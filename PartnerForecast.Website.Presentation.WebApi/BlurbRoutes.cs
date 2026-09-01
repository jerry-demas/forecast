

using PartnerForecast.Website.Application.Features.Blurbs;

namespace PartnerForecast.Website.Presentation.WebApi;

public static class BlurbRoutes
{
    public static RouteGroupBuilder MapPublicBlurbApi(this RouteGroupBuilder group)
    {
        group.MapGet("/", (HttpContext httpContext) =>
        {
            BlurbService service = httpContext.RequestServices.GetRequiredService<BlurbService>();

            IResult result = Results.Problem();
            service.GetPublicBlurbs().Match(
                success =>
                {
                    result = Results.Ok(success);
                },
                forFailure: (_, ex) =>
                {
                    result = Results.Problem();
                });

            return result;
        })
       .WithName("PublicGetBlurb");

        return group;
    }

    public static RouteGroupBuilder MapUserBlurbApi(this RouteGroupBuilder group)
    {
        group.MapGet("/", (HttpContext httpContext) =>
        {
            BlurbService service = httpContext.RequestServices.GetRequiredService<BlurbService>();
            
            IResult result = Results.Problem();
            service.GetUserBlurbs(httpContext.User.Identity?.Name ?? "").Match(
                success =>
                {
                    result = Results.Ok(success);
                },
                forFailure: (_, ex) =>
                {
                    result = Results.Problem();
                });

            return result;
        })
       .WithName("UserBlurbGet");


        group.MapPost("/", (HttpContext httpContext, string blurbText, bool isPrivate = false) =>
        {
            var tempBlurb = new Blurb
            {
                Text = blurbText,
                IsPrivate = isPrivate,
                CreatedBy = httpContext.User.Identity?.Name ?? "",
            };

            BlurbService service = httpContext.RequestServices.GetRequiredService<BlurbService>();

            IResult result = Results.Problem();
            service.CreateBlurb(tempBlurb).Match(
                success =>
                {
                    result = Results.Ok(success);
                },
                forFailure: (_, ex) =>
                {
                    result = Results.Problem();
                });

            return result;
        })
        .WithName("UserBlurbPost");

        return group;
    }

    public static RouteGroupBuilder MapBlurbApi(this RouteGroupBuilder group)
    {
        group.MapGet("/", (HttpContext httpContext, Guid id) =>
        {
            BlurbService service = httpContext.RequestServices.GetRequiredService<BlurbService>();

            IResult result = Results.Problem();
            service.GetBlurb(id).Match(
                success =>
                {
                    result = Results.Ok(success);
                },
                forFailure: (_, ex) =>
                {
                    if (ex is BlurbNotFoundException)
                    {
                        result = Results.NotFound(id);
                    }
                    else
                    { 
                        result = Results.Problem();
                    }
                });

            return result;
        })
       .WithName("BlurbGet");


        group.MapPut("/", (HttpContext httpContext, Guid id, string blurbText, bool isPrivate = false) =>
        {
            var tempBlurb = new Blurb
            {
                BlurbId = id,
                Text = blurbText,
                IsPrivate = isPrivate,
            };

            BlurbService service = httpContext.RequestServices.GetRequiredService<BlurbService>();

            IResult result = Results.Problem();
            service.UpdateBlurb(tempBlurb).Match(
                success =>
                {
                    result = Results.Ok(success);
                },
                forFailure: (_, ex) =>
                {
                    if (ex is BlurbNotFoundException)
                    {
                        result = Results.NotFound(id);
                    }
                    else
                    {
                        result = Results.Problem();
                    }
                });

            return result;
        })
        .WithName("BlurbPut");

                   
        group.MapDelete("/", (HttpContext httpContext, Guid id) =>
        {
            BlurbService service = httpContext.RequestServices.GetRequiredService<BlurbService>();
            
            IResult result = Results.Problem();
            service.DeleteBlurb(id).Match(
                success =>
                {
                    result = Results.Ok(success);
                },
                forFailure: (_, ex) =>
                {
                    if (ex is BlurbNotFoundException)
                    {
                        result = Results.NotFound(id);
                    }
                    else
                    {
                        result = Results.Problem();
                    }
                });

            return result;
        })
        .WithName("BlurbDelete");

        return group;
    }
}
