using Cbiz.SharedPackages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PartnerForecast.Website.Application.Features.Dice;

public static class DiceServiceExtensions
{
    public static IServiceCollection AddDiceService(this IServiceCollection services)
    {
        services.AddScoped<DiceService>();
        return services;
    }
}

public class DiceService
{
    private readonly ILogger<DiceService> _logger;

    public DiceService(ILogger<DiceService> logger)
    {
        _logger = logger;
    }

    public Either<List<int>, Exception> RollDice(DiceRollReuqest request)
    {
        _logger.LogInformation("Rolling a d{DiceSize} {NumberOfDice} times", request.DiceSize, request.NumberOfDice);
        List<int> results = new List<int>();
        for (int i = 0; i < request.NumberOfDice; i++)
        {            
            Random random = new Random();
            int result = random.Next(1, request.DiceSize + 1);
            results.Add(result);
        }
        return results;
    }
}
public interface IDiceRollReuqest
{
    public int DiceSize { get; }

    public int NumberOfDice { get; }

}
public record DiceRollReuqest(int DiceSize, int NumberOfDice)
{
    private const int MIN_DICE_SIZE = 1;
    private const int MAX_DICE_SIZE = 100;
    public int DiceSize { get; } =
    IsSizeValid(DiceSize) ? DiceSize : throw new DiceRequestException($"Dice size must be greater than {MIN_DICE_SIZE} and less than or equal to {MAX_DICE_SIZE}");

    private const int MIN_NUMBER_DICE = 1;
    private const int MAX_NUMBER_DICE = 10;
    public int NumberOfDice { get; } =
    IsNumberDiceValid(NumberOfDice) ? NumberOfDice : throw new DiceRequestException($"Number of dice must be atleast {MIN_NUMBER_DICE} and less than or equal to {MAX_NUMBER_DICE}");


    public static bool IsSizeValid(int DiceSize) => DiceSize > MIN_DICE_SIZE && DiceSize <= MAX_DICE_SIZE;
    public static bool IsNumberDiceValid(int NumberOfDice) => NumberOfDice >= MIN_NUMBER_DICE && NumberOfDice <= MAX_NUMBER_DICE;

    public static implicit operator DiceRollReuqest(int size) => new DiceRollReuqest(size, 1);
    public static DiceRollReuqest From(IDiceRollReuqest request) => new DiceRollReuqest(request.DiceSize, request.NumberOfDice);

}

public class DiceRequestException (string msg) : Exception(msg);