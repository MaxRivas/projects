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
    /// Edge-case integration tests for <see cref="MoviesApi.Controllers.MoviesController"/>.
    /// These tests validate invalid input and non-existent resource behaviours.
    /// They are intentionally minimal and rely on the same test-host pattern as other integration tests.
    /// </summary>
    public class MoviesControllerEdgeCasesTests : IClassFixture<WebApplicationFactory<MoviesApi.Program>>
    {
        private readonly WebApplicationFactory<MoviesApi.Program> _factory;


        private static UpdateMovieDto ValidUpdateMovieDto()
        {
            return new UpdateMovieDto
            {
                Id = 9999,
                Title = Constants.TestMovieTitle,
                Director = Constants.TestMovieDirector,
                ReleaseYear = Constants.TestMovieReleaseYear,
                Genre = Constants.TestMovieGenre
            };
        }
        private static CreateMovieDto ValidCreateMovieDto()
        {
            return new CreateMovieDto
            {
                Title = Constants.TestMovieTitle,
                Director = Constants.TestMovieDirector,
                ReleaseYear = Constants.TestMovieReleaseYear,
                Genre = Constants.TestMovieGenre
            };
        }

    

        public MoviesControllerEdgeCasesTests(WebApplicationFactory<MoviesApi.Program> factory)
        {
            // Use a unique in-memory DB name per test class to isolate state between runs.
            var uniqueDbName = "EdgeCaseTest_MoviesDb_" + Guid.NewGuid().ToString("N");
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseSetting(Constants.InMemoryDbNameKey, uniqueDbName);

                // Disable HTTPS redirect to reduce test log noise
                builder.UseSetting(Constants.EnableHttpsRedirectKey, "false");
            });

            // Ensure the in-memory database is created prior to running tests
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MovieContext>();
                db.Database.EnsureCreated();
            }
        }

        /// <summary>
        /// Sending an invalid movie model (required fields missing or out-of-range) should return BadRequest.
        /// This asserts model validation is wired and that controller responds with 400 for invalid payloads.
        /// </summary>
        [Fact]
        public async Task Create_InvalidModel_ReturnsBadRequest()
        {
            var client = _factory.CreateClient();

            // Create payload intentionally invalid: empty strings and too-small ReleaseYear
            var newMovie = new CreateMovieDto
            {
                Title = string.Empty,
                Director = string.Empty,
                ReleaseYear = -1800,
                Genre = string.Empty
            };

            var response = await client.PostAsJsonAsync(Constants.SlashMovies, newMovie);
            Assert.Equal(BadRequest, response.StatusCode);
        }

        /// <summary>
        /// Updating a non-existent movie must return NotFound (404).
        /// Verifies controller handles missing resources gracefully.
        /// </summary>
        [Fact]
        public async Task Update_Nonexistent_ReturnsNotFound()
        {
            var client = _factory.CreateClient();
            var newMovie = ValidUpdateMovieDto();

            var response = await client.PutAsJsonAsync($"/Movies/{newMovie.Id}", newMovie);
            Assert.Equal(NotFound, response.StatusCode);
        }

        /// <summary>
        /// Patching with an invalid ReleaseYear should return BadRequest; controller validates fields provided in Patch DTO.
        /// </summary>
        [Fact]
        public async Task Patch_InvalidReleaseYear_ReturnsBadRequest()
        {
            var client = _factory.CreateClient();

            // Create a valid movie first, then attempt an invalid partial update
            var newMovie = ValidUpdateMovieDto();
            var response = await client.PostAsJsonAsync(Constants.SlashMovies, newMovie);
            var created = await response.Content.ReadFromJsonAsync<MovieDto>();

            var patch = new PatchMovieDto { ReleaseYear = 1400 };
            response = await client.PatchAsJsonAsync($"/Movies/{created!.Id}", patch);
            Assert.Equal(BadRequest, response.StatusCode);
        }

        /// <summary>
        /// Create a movie with boundary values (very long title and lower ReleaseYear bound) to exercise edge conditions.
        /// </summary>
        [Fact]
        public async Task Create_BoundaryValues_Succeeds()
        {
            var client = _factory.CreateClient();

            // ReleaseYear lower bound and long title
            var releaseYear = 1888;// ReleaseYear lower bound and long title
            var longTitle = new string('A', 512);
            var newMovie = new CreateMovieDto 
            {
                Title = longTitle,
                Director = Constants.TestMovieDirector,
                ReleaseYear = releaseYear,
                Genre = Constants.TestMovieGenre
            };
            var response = await client.PostAsJsonAsync(Constants.SlashMovies, newMovie);

            Assert.Equal(Created, response.StatusCode);

            var created = await response.Content.ReadFromJsonAsync<MovieDto>();
            Assert.NotNull(created);
            Assert.Equal(releaseYear, created!.ReleaseYear);
            Assert.Equal(longTitle, created.Title);
        }
    
        /// <summary>
        /// Patch should only update the fields provided in the Patch DTO and leave other fields unchanged.
        /// </summary>
        [Fact]
        public async Task Patch_PartialUpdate_OnlyUpdatesSpecifiedFields()
        {
            var client = _factory.CreateClient();

            // Create an initial movie
            var newMovie = ValidUpdateMovieDto();
            var createResp = await client.PostAsJsonAsync(Constants.SlashMovies, newMovie);
            createResp.EnsureSuccessStatusCode();
            var created = await createResp.Content.ReadFromJsonAsync<MovieDto>();

            // Patch only the Title
            var newTitle = "Updating the Title only";
            var patch = new PatchMovieDto { Title = newTitle };
            var patchResp = await client.PatchAsJsonAsync($"/Movies/{created!.Id}", patch);
            Assert.Equal(NoContent, patchResp.StatusCode);

           var response = await client.GetFromJsonAsync<MovieDto>($"/Movies/{created.Id}");
            Assert.Equal(newTitle, response!.Title);
            Assert.Equal(Constants.TestMovieDirector, response.Director); // unchanged
        }

        /// <summary>
        /// Re-assert invalid patch handling with different bad year to ensure validation is consistent.
        /// </summary>
        [Fact]
        public async Task Patch_InvalidReleaseYear_ReturnsBadRequest_DifferentCase()
        {
            var client = _factory.CreateClient();

            // Create an initial movie
            var newMovie = ValidCreateMovieDto();
            var createResp = await client.PostAsJsonAsync(Constants.SlashMovies, newMovie);
            createResp.EnsureSuccessStatusCode();
            var response = await createResp.Content.ReadFromJsonAsync<MovieDto>();

            var patch = new PatchMovieDto { ReleaseYear = 1700 };
            var patchResp  = await client.PatchAsJsonAsync($"/Movies/{response!.Id}", patch);
            Assert.Equal(BadRequest, patchResp.StatusCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Update_NonExistentMovie_ReturnsNotFound()
        {
            var client = _factory.CreateClient();

            // Create an initial movie
            var newMovie = ValidUpdateMovieDto();
            var response = await client.PutAsJsonAsync($"/Movies/{newMovie.Id}", newMovie);

            Assert.Equal(NotFound, response.StatusCode);
        }
    }
}
