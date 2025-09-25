using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Models;
using Scalar.AspNetCore;

namespace MoviesApi
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // Configure JsonSerializerOptions for System.Text.Json
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.WriteIndented = true; 
                });

            builder.Services.AddControllersWithViews();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            // Add services to the container.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MoviesApp", builder => builder
                    .WithOrigins("http://localhost:5173", "https://localhost:5173") // Allow requests from Vite development server
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

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

                app.MapOpenApi(); // Map OpenAPI endpoints
                app.MapScalarApiReference(); // Maps the Scalar UI endpoint
                app.UseDeveloperExceptionPage();
            }          

            app.UseCors("MoviesApp"); // Apply the CORS policy
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();
            
            app.MapFallbackToFile("index.html"); // Serve the React client application
            app.Run();
        }
    }
}