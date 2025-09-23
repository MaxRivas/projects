using Microsoft.EntityFrameworkCore;
using MoviesApi.Models;

namespace MoviesApi
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            // Allow overriding the InMemory DB name via configuration (useful for tests)
            var inMemoryDbName = builder.Configuration["InMemoryDbName"] ?? "MoviesList";
            builder.Services.AddDbContext<MovieContext>(opt =>
                opt.UseInMemoryDatabase(inMemoryDbName));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

                app.MapOpenApi();

                // Enable swagger UI
                app.UseSwaggerUi(options =>
                {
                    options.DocumentPath = "/openapi/v1.json";
                });
            }

            // Only use HTTPS redirection when explicitly enabled (tests may disable this to avoid log noise)
            var enableHttpsRedirect = app.Configuration.GetValue<bool?>("EnableHttpsRedirect") ?? true;
            if (enableHttpsRedirect)
            {
                app.UseHttpsRedirection();
            }

            app.MapStaticAssets();

            // This middleware is crucial for serving the React client application.
            app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/api"), api =>
            {
                api.UseRouting();
                api.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (app.Environment.IsDevelopment())
                {
                    // Proxy requests to the Vite development server.
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:5173"); 
                }
            });

            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}