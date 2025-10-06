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

namespace MoviesApi.Controllers
{
    /// <summary>
    /// Integration tests for <see cref="MoviesApi.Controllers.MoviesController"/> using WebApplicationFactory.
    /// These tests exercise the HTTP endpoints end-to-end against the in-memory EF Core provider.
    /// </summary>
    public partial class MoviesControllerTests
    {
        /// <summary>
        /// Full CRUD flow smoke test: Create -> List -> Get -> Update -> Delete.
        /// Ensures endpoints return expected status codes and persisted values.
        /// </summary>
        [Fact]
        public async Task MoviesController_CrudFlow_Works()
        {
            var client = _baseFactory.CreateClient();

            // Create a new movie via POST
            var create = Helper.ValidCreateMovieDto();
            var createResp = await client.PostAsJsonAsync(Constants.SlashMovies, create);
            Assert.Equal(Created, createResp.StatusCode);

            var created = await createResp.Content.ReadFromJsonAsync<MovieDto>();
            Assert.NotNull(created);
            Assert.True(created!.Id > 0);

            // Read - list all movies and verify created item is present
            var list = await client.GetFromJsonAsync<MovieDto[]>(Constants.SlashMovies);
            Assert.NotNull(list);
            Assert.Contains(list!, m => m.Id == created.Id);

            var updateEndpoint = $"{Constants.SlashMovies}{Constants.Slash}{created.Id}";
            var getResp = await client.GetAsync(updateEndpoint);
            Assert.Equal(OK, getResp.StatusCode);
            var got = await getResp.Content.ReadFromJsonAsync<MovieDto>();
            Assert.Equal(Constants.TestMovieTitle, got!.Title);

            // Update the movie via PUT and verify change persisted
            var title = "Updated Movie";
            var update = Helper.ValidUpdateMovieDto();
            update.Id = created.Id;
            update.Title = title;

            var updateResp = await client.PutAsJsonAsync(updateEndpoint, update);
            Assert.Equal(NoContent, updateResp.StatusCode);

            var getAfterUpdate = await client.GetFromJsonAsync<MovieDto>(updateEndpoint);
            Assert.Equal(title, getAfterUpdate!.Title);

            // Delete the movie and verify it's removed
            var deleteResp = await client.DeleteAsync(updateEndpoint);
            Assert.Equal(NoContent, deleteResp.StatusCode);

            var getAfterDelete = await client.GetAsync(updateEndpoint);
            Assert.Equal(NotFound, getAfterDelete.StatusCode);
        }
    }
}
