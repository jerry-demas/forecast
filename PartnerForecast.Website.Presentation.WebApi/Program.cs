

using PartnerForecast.Website.Presentation.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.ConfigureServices();

// Build the application
var app = builder.Build();

// Configure middleware pipeline
app.ConfigureMiddleware(builder.Configuration);

app.Run();

/*

namespace PartnerForecast.Website.Presentation.WebApi;
public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
            .AddNegotiate();

        builder.Services.AddAuthorization(options =>
        {
            // By default, all incoming requests will be authorized according to the default policy.
            options.FallbackPolicy = options.DefaultPolicy;
        });

        builder.Services.AddBlurbService();
        builder.Services.AddDiceService();

        

        // Alter NLOG section in "appsettings.config" file in this project to change logging behaviors
        // See https://nlog-project.org/ for more information
        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddNLog(new NLogLoggingConfiguration(builder.Configuration.GetSection("NLog")));
        });

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.SetIsOriginAllowed(origin =>
                    {
                        // Allow any origin in development except for authenticated localhost
                        // If the origin is localhost, only allow if the request is authenticated
                        if (origin.StartsWith("http://localhost") || origin.StartsWith("https://localhost"))
                        {
                            // For localhost, require credentials (authentication)
                            return true;
                        }
                        // Allow all other origins (for development)
                        return true;
                    }).AllowCredentials()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
        }

        var app = builder.Build();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors(); // <-- Add this line to enable CORS middleware in the pipeline
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        

        //minimal APIs
        app.MapGroup("/api/public/blurb").MapPublicBlurbApi().AllowAnonymous().WithTags("Public");
        app.MapGroup("/api/user/blurb").MapUserBlurbApi().RequireAuthorization().WithTags("User");
        app.MapGroup("/api/blurb/{id}").MapBlurbApi().RequireAuthorization().WithTags("Blurb");

        app.MapGroup("/api/diceRoll").MapDiceApi().RequireAuthorization().WithTags("DiceRoll");

        app.Run();
    }
}

*/