using System;
using System.Text;
using System.Linq;
using System.Net.Http;
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
    public partial class MoviesControllerTests
    {
        /// <summary>
        /// Sending an invalid movie model (required fields missing or out-of-range) should return BadRequest.
        /// This asserts model validation is wired and that controller responds with 400 for invalid payloads.
        /// </summary>
        [Fact]
        public async Task MoviesController_Create_InvalidModel_ReturnsBadRequest()
        {
            var client = _baseFactory.CreateClient();

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
        public async Task MoviesController_Update_Nonexistent_ReturnsNotFound()
        {
            var client = _baseFactory.CreateClient();
            var newMovie = Helper.ValidUpdateMovieDto();

            var response = await client.PutAsJsonAsync($"{Constants.SlashMovies}{Constants.Slash}{newMovie.Id}", newMovie);
            Assert.Equal(NotFound, response.StatusCode);
        }

        /// <summary>
        /// Create a movie with boundary values (very long title and lower ReleaseYear bound) to exercise edge conditions.
        /// </summary>
        [Fact]
        public async Task MoviesController_Create_BoundaryValues_Succeeds()
        {
            var client = _baseFactory.CreateClient();

            // ReleaseYear lower bound and long title
            var releaseYear = Constants.ReleaseYearMin;// ReleaseYear lower bound and long title
            var longTitle = Helper.MakeString(512);
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
        public async Task MoviesController_Patch_PartialUpdate_OnlyUpdatesSpecifiedFields()
        {
            var client = _baseFactory.CreateClient();

            // Create an initial movie
            var newMovie = Helper.ValidUpdateMovieDto();
            var createResp = await client.PostAsJsonAsync(Constants.SlashMovies, newMovie);
            createResp.EnsureSuccessStatusCode();
            var created = await createResp.Content.ReadFromJsonAsync<MovieDto>();
            Assert.NotNull(created);

            // Patch only the Title
            string patchEndpoint = $"{Constants.SlashMovies}{Constants.Slash}{created.Id}";
            var patch = new PatchMovieDto { Title = Constants.NewTitle };
            var patchResp = await client.PatchAsJsonAsync(patchEndpoint, patch);
            Assert.Equal(NoContent, patchResp.StatusCode);

            var response = await client.GetFromJsonAsync<MovieDto>(patchEndpoint);
            Assert.NotNull(response);
            Assert.Equal(Constants.NewTitle, response.Title);
            Assert.Equal(Constants.TestMovieDirector, response.Director); // unchanged
        }

        /// <summary>
        /// Re-assert invalid patch handling with different bad year to ensure validation is consistent.
        /// </summary>
        [Fact]
        public async Task MoviesController_Patch_InvalidReleaseYear_ReturnsBadRequest_DifferentCase()
        {
            var client = _baseFactory.CreateClient();

            // Create an initial movie
            var newMovie = Helper.ValidCreateMovieDto();
            var createResp = await client.PostAsJsonAsync(Constants.SlashMovies, newMovie);
            createResp.EnsureSuccessStatusCode();
            var response = await createResp.Content.ReadFromJsonAsync<MovieDto>();
            Assert.NotNull(response);

            var patch = new PatchMovieDto { ReleaseYear = 1700 };
            var patchResp = await client.PatchAsJsonAsync($"{Constants.SlashMovies}{Constants.Slash}{response.Id}", patch);
            Assert.Equal(BadRequest, patchResp.StatusCode);
        }

        /// <summary>
        /// Updating a non-existent movie should return NotFound (404).
        /// This is a duplicate of Update_Nonexistent_ReturnsNotFound but kept for clarity.
        /// </summary>  
        [Fact]
        public async Task MoviesController_Update_NonExistentMovie_ReturnsNotFound()
        {
            var client = _baseFactory.CreateClient();

            // Create an initial movie
            var newMovie = Helper.ValidUpdateMovieDto();
            var response = await client.PutAsJsonAsync($"{Constants.SlashMovies}{Constants.Slash}{newMovie.Id}", newMovie);

            Assert.Equal(NotFound, response.StatusCode);
        }

        /// <summary>
        /// Patching a movie so it duplicates another movie should return 409 Conflict.
        /// </summary>
        [Fact]
        public async Task MoviesController_Patch_ToDuplicate_ReturnsConflict()
        {
            var client = _baseFactory.CreateClient();

            // Create movie A
            var createA = Helper.ValidCreateMovieDto();
            var respA = await client.PostAsJsonAsync(Constants.SlashMovies, createA);
            respA.EnsureSuccessStatusCode();
            var movieA = await respA.Content.ReadFromJsonAsync<MovieDto>();
            Assert.NotNull(movieA);

            // Create movie B with different data
            var createB = Helper.ValidCreateMovieDto();
            createB.Title = "Different Title";
            createB.Director = "Different Director";
            createB.ReleaseYear = createA.ReleaseYear + 1;
            var respB = await client.PostAsJsonAsync(Constants.SlashMovies, createB);
            respB.EnsureSuccessStatusCode();
            var movieB = await respB.Content.ReadFromJsonAsync<MovieDto>();
            Assert.NotNull(movieB);

            // Patch movie B to match movie A (should conflict)
            var patch = new PatchMovieDto { Title = movieA.Title, Director = movieA.Director, ReleaseYear = movieA.ReleaseYear };
            var patchResp = await client.PatchAsJsonAsync($"{Constants.SlashMovies}{Constants.Slash}{movieB.Id}", patch);
            Assert.Equal(Conflict, patchResp.StatusCode);
        }

        /// <summary>
        /// Prefill should be idempotent: calling it multiple times should not create duplicates.
        /// </summary>
        [Fact]
        public async Task MoviesController_Prefill_Idempotent()
        {
            var client = _baseFactory.CreateClient();
            const string prefillEndpoint = $"{Constants.SlashMovies}{Constants.Slash}{Constants.Prefill}";

            var r1 = await client.GetAsync(prefillEndpoint);
            Assert.Equal(System.Net.HttpStatusCode.OK, r1.StatusCode);

            var list1 = await client.GetFromJsonAsync<MovieDto[]>(Constants.SlashMovies);
            Assert.NotNull(list1);

            // Call Prefill again and ensure no new duplicates were added
            var r2 = await client.GetAsync(prefillEndpoint);
            Assert.Equal(System.Net.HttpStatusCode.OK, r2.StatusCode);
            var list2 = await client.GetFromJsonAsync<MovieDto[]>(Constants.SlashMovies);
            Assert.NotNull(list2);
            Assert.Equal(list1!.Length, list2.Length);
        }

        /// <summary>
        /// Duplicate detection should be case-insensitive.
        /// </summary>
        [Fact]
        public async Task MoviesController_Create_Duplicate_CaseInsensitive_ReturnsConflict()
        {
            var client = _baseFactory.CreateClient();


            var first = new CreateMovieDto
            {
                Title = Constants.TestMovieTitle,
                Director = Constants.TestMovieDirector,
                ReleaseYear = Constants.TestMovieReleaseYear,
                Genre = Constants.TestMovieGenre
            };
            var r1 = await client.PostAsJsonAsync(Constants.SlashMovies, first);
            Assert.Equal(System.Net.HttpStatusCode.Created, r1.StatusCode);

            var second = new CreateMovieDto
            {
                Title = Constants.TestMovieTitle.ToLower(),
                Director = Constants.TestMovieDirector.ToLower(),
                ReleaseYear = Constants.TestMovieReleaseYear,
                Genre = Constants.TestMovieGenre.ToLower()
            };
            var r2 = await client.PostAsJsonAsync(Constants.SlashMovies, second);
            Assert.Equal(Conflict, r2.StatusCode);
        }

        /// <summary>
        /// Sending a JSON literal null for PATCH should return BadRequest (patch==null).
        /// </summary>
        [Fact]
        public async Task MoviesController_Patch_NullBody_ReturnsBadRequest()
        {
            var client = _baseFactory.CreateClient();

            // Create a movie to patch
            var createResp = await client.PostAsJsonAsync(Constants.SlashMovies, Helper.ValidCreateMovieDto());
            createResp.EnsureSuccessStatusCode();
            var created = await createResp.Content.ReadFromJsonAsync<MovieDto>();

            // Send PATCH with a JSON null literal as the body
            var req = new HttpRequestMessage(new HttpMethod("PATCH"), $"{Constants.SlashMovies}{Constants.Slash}{created!.Id}")
            {
                Content = new StringContent("null", Encoding.UTF8, "application/json")
            };

            var resp = await client.SendAsync(req);
            Assert.Equal(BadRequest, resp.StatusCode);
        }
    }
}
