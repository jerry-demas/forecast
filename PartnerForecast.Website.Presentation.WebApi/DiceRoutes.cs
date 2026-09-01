using PartnerForecast.Website.Application.Features.Dice;
using System.ComponentModel;

namespace PartnerForecast.Website.Presentation.WebApi;

public static class DiceRoutes
{
     public class Request : IDiceRollReuqest
    {
        [DefaultValue(6)] // Sets default value for Swagger UI
        public int DiceSize { get; init; }

        [DefaultValue(1)] // Sets default value for Swagger UI
        public int NumberOfDice { get; init; }
    }


    public static RouteGroupBuilder MapDiceApi(this RouteGroupBuilder group)
    {
        group.MapPost("/", (HttpContext httpContext, Request request) =>
        {
            DiceService service = httpContext.RequestServices.GetRequiredService<DiceService>();

            IResult result = Results.Problem();

            try
            {
                service.RollDice(DiceRollReuqest.From(request)).Match(
                    success =>
                    {
                        result = Results.Ok(success);
                    },
                    forFailure: (_, ex) =>
                    {
                        result= Results.Problem(ex switch
                        {
                            DiceRequestException => $"Dice request error: {ex.Message}",
                            _ => "An issue has occured with DiceService please contact support."
                        });
                    }
                );
            }
            catch (Exception ex) //handle any unhandled exceptions in the logic loop 
            {
                return ex switch
                {
                     DiceRequestException => Results.BadRequest($"Dice request error: {ex.Message}"),
                     _ => Results.Problem("An issue has occured with DiceService please contact support.")
                };                
            }

            return result;
        })
       .WithName("DiceRollGet");

        return group;
    }
}
