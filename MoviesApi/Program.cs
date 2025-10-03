using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoviesApi.Models;
using Scalar.AspNetCore;
// Optional KeyVault support (install packages if you want this to run):
using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;

namespace MoviesApi
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Load user-secrets in development so local Jwt:Key can be stored securely
            if (builder.Environment.IsDevelopment())
            {
                // optional: do not throw if no user-secrets present
                builder.Configuration.AddUserSecrets<Program>(optional: true);
            }

            // If a Key Vault URI is configured, add it to configuration (optional)
            var keyVaultUri = builder.Configuration["KeyVault:VaultUri"];
            if (!string.IsNullOrWhiteSpace(keyVaultUri))
            {
                try
                {
                    var credential = new DefaultAzureCredential();
                    // Key Vault provider is added last so Key Vault secrets override earlier sources
                    ((IConfigurationBuilder)builder.Configuration).AddAzureKeyVault(new Uri(keyVaultUri), credential);
                }
                catch
                {
                    // If Key Vault packages are not available or auth fails, continue with existing config.
                }
            }

            // JWT settings from configuration (user-secrets / environment / KeyVault)
            var jwtSection = builder.Configuration.GetSection("Jwt");
            var jwtKey = jwtSection["Key"] ?? "ChangeThisDevSigningKey_ReplaceInProd";
            var jwtIssuer = jwtSection["Issuer"] ?? "moviesapi";
            var jwtExpireMinutes = int.TryParse(jwtSection["ExpireMinutes"], out var m) ? m : 60;

            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.WriteIndented = true;
                });

            builder.Services.AddControllersWithViews();

            // OpenAPI wiring (keep available only in Development by default)
            builder.Services.AddOpenApi();

            // Authentication & Authorization (require JWT for all endpoints by default)
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtIssuer,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ValidateIssuerSigningKey = true
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                // Require authenticated user for all endpoints unless [AllowAnonymous] is used
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                // Additional role-based policy example
                options.AddPolicy("AdminOnly", p => p.RequireRole("Admin"));
            });

            // CORS policy for the Vite dev server
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MoviesApp", cors =>
                    cors.WithOrigins("http://localhost:5173", "https://localhost:5173")
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
                app.UseHsts();
                app.UseDeveloperExceptionPage();

                // Protect OpenAPI/Swagger UI with authentication in development:
                // If a request targets the OpenAPI or Swagger routes, require authentication.
                app.Use(async (context, next) =>
                {
                    var path = context.Request.Path.Value ?? string.Empty;
                    if (path.StartsWith("/openapi", StringComparison.OrdinalIgnoreCase)
                        || path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase)
                        || path.StartsWith("/nswag", StringComparison.OrdinalIgnoreCase)
                        || path.StartsWith("/scalar", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!context.User.Identity?.IsAuthenticated ?? true)
                        {
                            // challenge the request (will return 401/redirect to auth depending on scheme)
                            await context.ChallengeAsync();
                            return;
                        }
                    }
                    await next();
                });

                // Map OpenAPI endpoints (UI + JSON) - still mapped, but above middleware requires auth to view them
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseCors("MoviesApp");
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("index.html"); // Serve the React client application
            app.Run();
        }
    }
}