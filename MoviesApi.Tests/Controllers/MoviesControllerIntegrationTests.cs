using System;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoviesApi.Models;
using MoviesApi.Dto;
using Xunit;
using MoviesApi.Tests.Utilities;
using static System.Net.HttpStatusCode;

namespace MoviesApi.Tests.Controllers
{
    /// <summary>
    /// Integration tests for <see cref="MoviesApi.Controllers.MoviesController"/> using WebApplicationFactory.
    /// These tests exercise the HTTP endpoints end-to-end against the in-memory EF Core provider.
    /// </summary>
    public class MoviesControllerIntegrationTests : IClassFixture<WebApplicationFactory<MoviesApi.Program>>
    {
        #region Private Fields

        private readonly WebApplicationFactory<MoviesApi.Program> _factory;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor that sets up a test host with a unique in-memory database for isolation.
        /// This ensures tests do not interfere with each other's state.
        /// The database is created fresh for each test class instance.
        /// </summary>
        public MoviesControllerIntegrationTests(WebApplicationFactory<MoviesApi.Program> factory)
        {
            // Create a factory that sets an override InMemoryDbName so the app will use that DB instance
            var uniqueDbName = "IntegrationTest_MoviesDb_" + Guid.NewGuid().ToString(Constants.N);
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseSetting(Constants.InMemoryDbNameKey, uniqueDbName);
                // Disable HTTPS redirect during tests to avoid "Failed to determine the https port for redirect" log noise
                builder.UseSetting(Constants.EnableHttpsRedirectKey, Constants.False);
                // Ensure environment is Development so swagger remains enabled if needed
                builder.UseSetting(Constants.EnvironmentKey, Constants.Development);
            });

            // Ensure the database is created for the actual test server's service provider
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MovieContext>();
                db.Database.EnsureCreated();
            }
        }

        #endregion

        /// <summary>
        /// Full CRUD flow smoke test: Create -> List -> Get -> Update -> Delete.
        /// Ensures endpoints return expected status codes and persisted values.
        /// </summary>
        [Fact]
        public async Task CrudFlow_Works()
        {
            var client = _factory.CreateClient();

            // Create a new movie via POST
            var create = Helper.ValidCreateMovieDto();
            var createResp = await client.PostAsJsonAsync(Constants.SlashMovies, create);
            Assert.Equal(System.Net.HttpStatusCode.Created, createResp.StatusCode);

            var created = await createResp.Content.ReadFromJsonAsync<MovieDto>();
            Assert.NotNull(created);
            Assert.True(created!.Id > 0);

            // Read - list all movies and verify created item is present
            var list = await client.GetFromJsonAsync<MovieDto[]>(Constants.SlashMovies);
            Assert.NotNull(list);
            Assert.Contains(list!, m => m.Id == created.Id);

            var getResp = await client.GetAsync($"{Constants.SlashMovies}{Constants.Slash}{created!.Id}");
            Assert.Equal(System.Net.HttpStatusCode.OK, getResp.StatusCode);
            var got = await getResp.Content.ReadFromJsonAsync<MovieDto>();
            Assert.Equal(Constants.TestMovieTitle, got!.Title);

            // Update the movie via PUT and verify change persisted
            var title = "Updated Movie";
            var update = Helper.ValidUpdateMovieDto();
            update.Id = created.Id;
            update.Title = title;
            var updateResp = await client.PutAsJsonAsync($"{Constants.SlashMovies}{Constants.Slash}{created.Id}", update);
            Assert.Equal(System.Net.HttpStatusCode.NoContent, updateResp.StatusCode);

            var getAfterUpdate = await client.GetFromJsonAsync<MovieDto>($"{Constants.SlashMovies}{Constants.Slash}{created.Id}");
            Assert.Equal(title, getAfterUpdate!.Title);

            // Delete the movie and verify it's removed
            var deleteResp = await client.DeleteAsync($"{Constants.SlashMovies}{Constants.Slash}{created.Id}");
            Assert.Equal(System.Net.HttpStatusCode.NoContent, deleteResp.StatusCode);

            var getAfterDelete = await client.GetAsync($"{Constants.SlashMovies}{Constants.Slash}{created.Id}");
            Assert.Equal(System.Net.HttpStatusCode.NotFound, getAfterDelete.StatusCode);
        }
    }
}
