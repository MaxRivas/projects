using System;
using System.Text;
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
        #region Private Fields

            private readonly WebApplicationFactory<MoviesApi.Program> _factory;

        #endregion

        #region Constructor

            /// <summary>
            /// Constructor that sets up a test host with a unique in-memory database for isolation.
            /// This ensures tests do not interfere with each other's state.
            /// The database is created fresh for each test class instance.
            /// The in-memory database name is randomized using a GUID to avoid collisions.
            /// HTTPS redirection is disabled to reduce log noise during tests.
            /// The environment is set to Development to keep Swagger/OpenAPI available if needed.
            /// The database is ensured to be created before any tests run. 
            /// </summary>
            /// <param name="factory"></param>
            public MoviesControllerEdgeCasesTests(WebApplicationFactory<MoviesApi.Program> factory)
            {
                // Use a unique in-memory DB name per test class to isolate state between runs.
                var uniqueDbName = "EdgeCaseTest_MoviesDb_" + Guid.NewGuid().ToString(Constants.N);
                _factory = factory.WithWebHostBuilder(builder =>
                {
                    builder.UseSetting(Constants.InMemoryDbNameKey, uniqueDbName);
    
                    // Disable HTTPS redirect to reduce test log noise
                    builder.UseSetting(Constants.EnableHttpsRedirectKey, Constants.False);
                });
    
                // Ensure the in-memory database is created prior to running tests
                using (var scope = _factory.Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<MovieContext>();
                    db.Database.EnsureCreated();
                }
            }
            
        #endregion

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
            var newMovie = Helper.ValidUpdateMovieDto();

            var response = await client.PutAsJsonAsync($"{Constants.SlashMovies}{Constants.Slash}{newMovie.Id}", newMovie);
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
            var newMovie = Helper.ValidUpdateMovieDto();
            var response = await client.PostAsJsonAsync(Constants.SlashMovies, newMovie);
            var created = await response.Content.ReadFromJsonAsync<MovieDto>();

            var patch = new PatchMovieDto { ReleaseYear = 1400 };
            response = await client.PatchAsJsonAsync($"{Constants.SlashMovies}{Constants.Slash}{created!.Id}", patch);
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
            var newMovie = Helper.ValidUpdateMovieDto();
            var createResp = await client.PostAsJsonAsync(Constants.SlashMovies, newMovie);
            createResp.EnsureSuccessStatusCode();
            var created = await createResp.Content.ReadFromJsonAsync<MovieDto>();

            // Patch only the Title
            var newTitle = "Updating the Title only";
            var patch = new PatchMovieDto { Title = newTitle };
            var patchResp = await client.PatchAsJsonAsync($"{Constants.SlashMovies}{Constants.Slash}{created!.Id}", patch);
            Assert.Equal(NoContent, patchResp.StatusCode);

           var response = await client.GetFromJsonAsync<MovieDto>($"{Constants.SlashMovies}{Constants.Slash}{created.Id}");
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
            var newMovie = Helper.ValidCreateMovieDto();
            var createResp = await client.PostAsJsonAsync(Constants.SlashMovies, newMovie);
            createResp.EnsureSuccessStatusCode();
            var response = await createResp.Content.ReadFromJsonAsync<MovieDto>();

            var patch = new PatchMovieDto { ReleaseYear = 1700 };
            var patchResp  = await client.PatchAsJsonAsync($"{Constants.SlashMovies}{Constants.Slash}{response!.Id}", patch);
            Assert.Equal(BadRequest, patchResp.StatusCode);
        }

        /// <summary>
        /// Updating a non-existent movie should return NotFound (404).
        /// This is a duplicate of Update_Nonexistent_ReturnsNotFound but kept for clarity.
        /// </summary>  
        [Fact]
        public async Task Update_NonExistentMovie_ReturnsNotFound()
        {
            var client = _factory.CreateClient();

            // Create an initial movie
            var newMovie = Helper.ValidUpdateMovieDto();
            var response = await client.PutAsJsonAsync($"{Constants.SlashMovies}{Constants.Slash}{newMovie.Id}", newMovie);

            Assert.Equal(NotFound, response.StatusCode);
        }

        /// <summary>
        /// Creating a duplicate movie (same Title+Director+ReleaseYear) should return 409 Conflict.
        /// </summary>
        [Fact]
        public async Task Create_Duplicate_ReturnsConflict()
        {
            var client = _factory.CreateClient();

            var create = Helper.ValidCreateMovieDto();
            var first = await client.PostAsJsonAsync(Constants.SlashMovies, create);
            Assert.Equal(Created, first.StatusCode);

            // Attempt to create the same movie again
            var second = await client.PostAsJsonAsync(Constants.SlashMovies, create);
            Assert.Equal(Conflict, second.StatusCode);
        }

        /// <summary>
        /// Patching a movie so it duplicates another movie should return 409 Conflict.
        /// </summary>
        [Fact]
        public async Task Patch_ToDuplicate_ReturnsConflict()
        {
            var client = _factory.CreateClient();

            // Create movie A
            var createA = Helper.ValidCreateMovieDto();
            var respA = await client.PostAsJsonAsync(Constants.SlashMovies, createA);
            respA.EnsureSuccessStatusCode();
            var movieA = await respA.Content.ReadFromJsonAsync<MovieDto>();

            // Create movie B with different data
            var createB = Helper.ValidCreateMovieDto();
            createB.Title = "Different Title";
            createB.Director = "Different Director";
            createB.ReleaseYear = createA.ReleaseYear + 1;
            var respB = await client.PostAsJsonAsync(Constants.SlashMovies, createB);
            respB.EnsureSuccessStatusCode();
            var movieB = await respB.Content.ReadFromJsonAsync<MovieDto>();

            // Patch movie B to match movie A (should conflict)
            var patch = new PatchMovieDto { Title = movieA.Title, Director = movieA.Director, ReleaseYear = movieA.ReleaseYear };
            var patchResp = await client.PatchAsJsonAsync($"{Constants.SlashMovies}{Constants.Slash}{movieB!.Id}", patch);
            Assert.Equal(Conflict, patchResp.StatusCode);
        }

        /// <summary>
        /// Prefill should be idempotent: calling it multiple times should not create duplicates.
        /// </summary>
        [Fact]
        public async Task Prefill_Idempotent()
        {
            var client = _factory.CreateClient();

            var r1 = await client.GetAsync($"{Constants.SlashMovies}/Prefill");
            Assert.Equal(System.Net.HttpStatusCode.OK, r1.StatusCode);

            var list1 = await client.GetFromJsonAsync<MovieDto[]>(Constants.SlashMovies);
            Assert.NotNull(list1);

            // Call Prefill again and ensure no new duplicates were added
            var r2 = await client.GetAsync($"{Constants.SlashMovies}/Prefill");
            Assert.Equal(System.Net.HttpStatusCode.OK, r2.StatusCode);
            var list2 = await client.GetFromJsonAsync<MovieDto[]>(Constants.SlashMovies);
            Assert.Equal(list1!.Length, list2!.Length);
        }

        /// <summary>
        /// When the id in the route does not match the id in the body, the API should return BadRequest.
        /// </summary>
        [Fact]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            var client = _factory.CreateClient();

            // Create an initial movie to have a valid id
            var createResp = await client.PostAsJsonAsync(Constants.SlashMovies, Helper.ValidCreateMovieDto());
            createResp.EnsureSuccessStatusCode();
            var created = await createResp.Content.ReadFromJsonAsync<MovieDto>();

            var update = Helper.ValidUpdateMovieDto();
            update.Id = created!.Id; // body id matches created id

            // Call PUT with a different id in the route
            var badPut = await client.PutAsJsonAsync($"{Constants.SlashMovies}/{created.Id + 1}", update);
            Assert.Equal(BadRequest, badPut.StatusCode);
        }

        /// <summary>
        /// Duplicate detection should be case-insensitive.
        /// </summary>
        [Fact]
        public async Task Create_Duplicate_CaseInsensitive_ReturnsConflict()
        {
            var client = _factory.CreateClient();

            var first = new CreateMovieDto { Title = "CaseTest", Director = "Dir", ReleaseYear = 2001, Genre = "Drama" };
            var r1 = await client.PostAsJsonAsync(Constants.SlashMovies, first);
            Assert.Equal(System.Net.HttpStatusCode.Created, r1.StatusCode);

            var second = new CreateMovieDto { Title = "casetest", Director = "dir", ReleaseYear = 2001, Genre = "Drama" };
            var r2 = await client.PostAsJsonAsync(Constants.SlashMovies, second);
            Assert.Equal(Conflict, r2.StatusCode);
        }

        /// <summary>
        /// Sending a JSON literal null for PATCH should return BadRequest (patch==null).
        /// </summary>
        [Fact]
        public async Task Patch_NullBody_ReturnsBadRequest()
        {
            var client = _factory.CreateClient();

            // Create a movie to patch
            var createResp = await client.PostAsJsonAsync(Constants.SlashMovies, Helper.ValidCreateMovieDto());
            createResp.EnsureSuccessStatusCode();
            var created = await createResp.Content.ReadFromJsonAsync<MovieDto>();

            // Send PATCH with a JSON null literal as the body
            var req = new HttpRequestMessage(new HttpMethod("PATCH"), $"{Constants.SlashMovies}/{created!.Id}")
            {
                Content = new StringContent("null", Encoding.UTF8, "application/json")
            };

            var resp = await client.SendAsync(req);
            Assert.Equal(BadRequest, resp.StatusCode);
        }
    }
}
