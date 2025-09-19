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
            builder.Services.AddControllers();

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

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}