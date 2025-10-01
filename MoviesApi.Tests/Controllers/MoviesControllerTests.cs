using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoviesApi.Dto;
using MoviesApi.Models;
using Xunit;
using MoviesApi.Tests.Utilities;
using System.Text;

namespace MoviesApi.Controllers
{
    public partial class MoviesControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        #region Private Fields

        private readonly WebApplicationFactory<Program> _baseFactory;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MoviesControllerTest"/> class.
        /// </summary>
        /// <param name="baseFactory">The base factory.</param>
        public MoviesControllerTests(WebApplicationFactory<Program> baseFactory)
        {
            _baseFactory = baseFactory;
        }

        #endregion

        /// <summary>
        /// Creates a new factory for testing with a unique in-memory database.
        /// </summary>
        /// <param name="dbName">DB name for the in-memory database.</param>
        /// <returns>WebApplicationFactory instance for testing.</returns>
        private WebApplicationFactory<Program> CreateFactory(string dbName)
        {
            return _baseFactory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // remove existing MovieContext registration
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MovieContext>));
                    if (descriptor != null) services.Remove(descriptor);

                    // add in-memory MovieContext with unique db name
                    services.AddDbContext<MovieContext>(options =>
                    {
                        options.UseInMemoryDatabase(dbName);
                    });

                    // Build service provider & ensure DB created
                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<MovieContext>();
                    db.Database.EnsureCreated();
                });
            });
        }

        /// <summary>
        /// Creates a new HttpClient for testing with a unique in-memory database.
        /// </summary>
        /// <param name="dbName">DB name for the in-memory database.</param>
        /// <returns>HttpClient instance for making requests.</returns>
        private HttpClient CreateClient(string dbName)
        {
            var factory = CreateFactory(dbName);
            return factory.CreateClient()!;
        }

        /// <summary>
        /// Helper to create a movie via the API.
        /// </summary>
        /// <param name="client">HttpClient instance for making requests.</param>
        /// <param name="create">The movie creation DTO.</param>
        /// <returns>The created MovieDto.</returns>
        private async Task<MovieDto> CreateMovieAsync(HttpClient client, CreateMovieDto create)
        {
            var resp = await client.PostAsJsonAsync(Constants.SlashMovies, create);
            resp.EnsureSuccessStatusCode();
            var dto = await resp.Content.ReadFromJsonAsync<MovieDto>();
            if (dto is null)
            {
                throw new InvalidOperationException("Failed to deserialize MovieDto from create response.");
            }
            return dto;
        }
    }
}