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

namespace MoviesApi.Tests.Controllers
{
    /// <summary>
    /// Integration tests for <see cref="MoviesApi.Controllers.MoviesController"/> using WebApplicationFactory.
    /// These tests exercise the HTTP endpoints end-to-end against the in-memory EF Core provider.
    /// </summary>
    public class MoviesControllerIntegrationTests
        : IClassFixture<WebApplicationFactory<MoviesApi.Program>>
    {
        private readonly WebApplicationFactory<MoviesApi.Program> _factory;

        public MoviesControllerIntegrationTests(WebApplicationFactory<MoviesApi.Program> factory)
        {
            // Create a factory that sets an override InMemoryDbName so the app will use that DB instance
            var uniqueDbName = "IntegrationTest_MoviesDb_" + Guid.NewGuid().ToString("N");
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseSetting("InMemoryDbName", uniqueDbName);
                // Disable HTTPS redirect during tests to avoid "Failed to determine the https port for redirect" log noise
                builder.UseSetting("EnableHttpsRedirect", "false");
                // Ensure environment is Development so swagger remains enabled if needed
                builder.UseSetting("environment", "Development");
            });

            // Ensure the database is created for the actual test server's service provider
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MovieContext>();
                db.Database.EnsureCreated();
            }
        }

        /// <summary>
        /// Full CRUD flow smoke test: Create -> List -> Get -> Update -> Delete.
        /// Ensures endpoints return expected status codes and persisted values.
        /// </summary>
        [Fact]
        public async Task CrudFlow_Works()
        {
            var client = _factory.CreateClient();

        // Create a new movie via POST
            var create = new CreateMovieDto { Title = "New Movie", Director = "Director", ReleaseYear = 2024, Genre = "Drama" };
            var createResp = await client.PostAsJsonAsync("/Movies", create);
            Assert.Equal(System.Net.HttpStatusCode.Created, createResp.StatusCode);

            var created = await createResp.Content.ReadFromJsonAsync<MovieDto>();
            Assert.NotNull(created);
            Assert.True(created!.Id > 0);

        // Read - list all movies and verify created item is present
            var list = await client.GetFromJsonAsync<MovieDto[]>("/Movies");
            Assert.NotNull(list);
            Assert.Contains(list!, m => m.Id == created.Id);

            var getResp = await client.GetAsync($"/Movies/{created!.Id}");
            Assert.Equal(System.Net.HttpStatusCode.OK, getResp.StatusCode);
            var got = await getResp.Content.ReadFromJsonAsync<MovieDto>();
            Assert.Equal("New Movie", got!.Title);

        // Update the movie via PUT and verify change persisted
            var update = new UpdateMovieDto { Id = created.Id, Title = "Updated Movie", Director = "Director", ReleaseYear = 2025, Genre = "Drama" };
            var updateResp = await client.PutAsJsonAsync($"/Movies/{created.Id}", update);
            Assert.Equal(System.Net.HttpStatusCode.NoContent, updateResp.StatusCode);

            var getAfterUpdate = await client.GetFromJsonAsync<MovieDto>($"/Movies/{created.Id}");
            Assert.Equal("Updated Movie", getAfterUpdate!.Title);

        // Delete the movie and verify it's removed
            var deleteResp = await client.DeleteAsync($"/Movies/{created.Id}");
            Assert.Equal(System.Net.HttpStatusCode.NoContent, deleteResp.StatusCode);

            var getAfterDelete = await client.GetAsync($"/Movies/{created.Id}");
            Assert.Equal(System.Net.HttpStatusCode.NotFound, getAfterDelete.StatusCode);
        }
    }
}
