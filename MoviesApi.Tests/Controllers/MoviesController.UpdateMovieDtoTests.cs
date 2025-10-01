using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Controllers;
using MoviesApi.Models;
using MoviesApi.Dto;
using Xunit;
using MoviesApi.Tests.Utilities;
using System.Net;
using System.Net.Http.Json;

namespace MoviesApi.Controllers
{
    public partial class MoviesControllerTests
    {
        /// <summary>
        /// Tests successful update of a movie and verifies changes are persisted.
        /// </summary>
        [Fact]
        public async Task Update_Succeeds()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var movieDto = Helper.ValidCreateMovieDto();
            var created = await CreateMovieAsync(client, movieDto);

            const string updatedTitle = "UpdatedTitle";
            const string updatedGenre = "UpdatedGenre";
            var update = Helper.ValidUpdateMovieDto();
            update.Id = created.Id;
            update.Title = updatedTitle;
            update.Genre = updatedGenre;

            var updateEndpoint = $"{Constants.SlashMovies}{Constants.Slash}{created.Id}";
            var putResp = await client.PutAsJsonAsync(updateEndpoint, update);
            Assert.Equal(HttpStatusCode.NoContent, putResp.StatusCode);

            var get = await client.GetFromJsonAsync<MovieDto>(updateEndpoint);
            Assert.NotNull(get);
            Assert.Equal(updatedTitle, get.Title);
            Assert.Equal(updatedGenre, get.Genre);
        }

        /// <summary>
        /// Tests update of a non-existent movie causing a conflict.
        /// </summary>
        [Fact]
        public async Task Update_Conflict_ReturnsConflict()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var movie1 = Helper.ValidCreateMovieDto();
            var movie2 = Helper.ValidCreateMovieDto();

            // Make movie2 distinct so CreateMovieAsync does not return Conflict when inserting
            movie2.Title = movie2.Title + "_other";

            var movieDto1 = await CreateMovieAsync(client, movie1);
            var movieDto2 = await CreateMovieAsync(client, movie2);

            // Try to update movieDto2 to match movieDto1 -> should conflict
            var update = Helper.ValidUpdateMovieDto();
            update.Id = movieDto2.Id;
            update.Title = movieDto1.Title;
            update.Director = movieDto1.Director;
            update.ReleaseYear = movieDto1.ReleaseYear;
            update.Genre = movieDto1.Genre;

            var resp = await client.PutAsJsonAsync($"{Constants.SlashMovies}{Constants.Slash}{movieDto2.Id}", update);
            Assert.Equal(HttpStatusCode.Conflict, resp.StatusCode);
        }

        /// <summary>
        /// Tests update with ID mismatch returning BadRequest.
        /// </summary>
        [Fact]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var created = await CreateMovieAsync(client, Helper.ValidCreateMovieDto());

            var update = Helper.ValidUpdateMovieDto();
            update.Id = created.Id + 1; // mismatch intentionally

            var resp = await client.PutAsJsonAsync($"{Constants.SlashMovies}{Constants.Slash}{created.Id}", update);
            Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        }

        /// <summary>
        /// Tests update of a non-existent movie.
        /// </summary>
        [Fact]
        public async Task Update_NotFound_ReturnsNotFound()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var updateId = 99999;
            var update = Helper.ValidUpdateMovieDto();
            update.Id = updateId;

            var resp = await client.PutAsJsonAsync($"{Constants.SlashMovies}{Constants.Slash}{updateId}", update);
            Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
        }
    }
}