namespace PartnerForecast.Website.Presentation.WebApi.Extensions;

public static class WebApplicationExtensions
{

    public static WebApplication ConfigureMiddleware(this WebApplication app, IConfiguration configuration)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();

        // Enable CORS
        app.UseCors("ReactFrontend");

        app.UseStaticFiles();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        
        if (!app.Environment.IsDevelopment())
        {
            // Handle Next.js static routes - serve the correct index.html for each route
            app.MapFallback(async context =>
            {
                var path = context.Request.Path.Value?.TrimEnd('/') ?? "";

                // Try to serve the route-specific index.html (e.g., /create-portal/index.html)
                var routeFile = $"{path}/index.html";
                var fileInfo = app.Environment.WebRootFileProvider.GetFileInfo(routeFile);

                if (fileInfo.Exists)
                {
                    context.Response.ContentType = "text/html";
                    await context.Response.SendFileAsync(fileInfo);
                }
                else
                {
                    // Fallback to root index.html
                    context.Response.ContentType = "text/html";
                    var rootFile = app.Environment.WebRootFileProvider.GetFileInfo("index.html");
                    if (rootFile.Exists)
                    {
                        await context.Response.SendFileAsync(rootFile);
                    }
                }
            });
        }

        return app;
    }



}
