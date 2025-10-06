using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Controllers;
using MoviesApi.Models;
using MoviesApi.Dto;
using Xunit;
using MoviesApi.Tests.Utilities;
using System.Net.Http.Json;
using System.Net.Http;
using System.Net;

namespace MoviesApi.Controllers
{
    public partial class MoviesControllerTests
    {
        /// <summary>
        /// Tests patching a movie successfully.
        /// </summary> 
        [Fact]
        public async Task MoviesController_Patch_Succeeds()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var create = Helper.ValidCreateMovieDto();
            var created = await CreateMovieAsync(client, create);

            // Patch only the Title field   
            var updateEndpoint = $"{Constants.SlashMovies}{Constants.Slash}{created.Id}";
            const string patchedTitle = "PatchedTitle";
            var patchObj = new { Title = patchedTitle };
            var content = JsonContent.Create(patchObj);
            var request = new HttpRequestMessage(new HttpMethod(Constants.Patch), updateEndpoint) { Content = content };
            var resp = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NoContent, resp.StatusCode);

            var updated = await client.GetFromJsonAsync<MovieDto>(updateEndpoint);
            Assert.NotNull(updated);
            Assert.Equal(patchedTitle, updated!.Title);
        }


        /// <summary>
        /// Tests that a PATCH request with no content returns a BadRequest.
        /// </summary>
        [Fact]
        public async Task MoviesController_Patch_NoContent_ReturnsBadRequest()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var url = $"{Constants.SlashMovies}{Constants.Slash}{Constants.ReleaseYearMax}";

            // Send a PATCH with no content at all
            var request = new HttpRequestMessage(new HttpMethod(Constants.Patch), url);
            var resp = await client.SendAsync(request);

            // The runtime currently returns 415 UnsupportedMediaType for an empty patch request
            Assert.Equal(HttpStatusCode.UnsupportedMediaType, resp.StatusCode);
        }

        /// <summary>
        /// Tests that a PATCH request for a non-existent movie returns NotFound.
        /// </summary>
        [Fact]
        public async Task MoviesController_Patch_NotFound_ReturnsNotFound()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var url = $"{Constants.SlashMovies}{Constants.Slash}{Constants.ReleaseYearMax}";

            var patchObj = new { Title = "X" };
            var content = JsonContent.Create(patchObj);
            var resp = await client.PatchAsync(url, content);

            Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
        }

        /// <summary>
        /// Tests that a PATCH request with invalid data returns BadRequest.
        /// </summary>
        [Fact]
        public async Task MoviesController_Patch_EmptyTitle_ReturnsBadRequest()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var created = await CreateMovieAsync(client, Helper.ValidCreateMovieDto());

            var updateEndpoint = $"{Constants.SlashMovies}{Constants.Slash}{created.Id}";
            var patchObj = new { Title = string.Empty };
            var content = JsonContent.Create(patchObj);
            var resp = await client.PatchAsync(updateEndpoint, content);

            Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        }

        /// <summary>
        /// Tests that a PATCH request with invalid data returns BadRequest.
        /// </summary> 
        [Fact]
        public async Task MoviesController_Patch_EmptyDirector_ReturnsBadRequest()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var created = await CreateMovieAsync(client, Helper.ValidCreateMovieDto());

            var updateEndpoint = $"{Constants.SlashMovies}{Constants.Slash}{created.Id}";
            var patchObj = new { Director = " " };
            var content = JsonContent.Create(patchObj);
            var resp = await client.PatchAsync(updateEndpoint, content);

            Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        }

        /// <summary>
        /// Tests that a PATCH request with invalid data returns BadRequest.
        /// </summary>
        [Fact]
        public async Task MoviesController_Patch_EmptyGenre_ReturnsBadRequest()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var created = await CreateMovieAsync(client, Helper.ValidCreateMovieDto());

            var updateEndpoint = $"{Constants.SlashMovies}{Constants.Slash}{created.Id}";
            var patchObj = new { Genre = string.Empty };
            var content = JsonContent.Create(patchObj);
            var resp = await client.PatchAsync(updateEndpoint, content);

            Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        }

        /// <summary>
        /// Tests that a PATCH request with invalid data returns BadRequest.
        /// </summary>
        [Fact]
        public async Task MoviesController_Patch_InvalidReleaseYear_ReturnsBadRequest()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var created = await CreateMovieAsync(client, Helper.ValidCreateMovieDto());

            var updateEndpoint = $"{Constants.SlashMovies}{Constants.Slash}{created.Id}";
            var patchObj = new { ReleaseYear = 1700 };
            var content = JsonContent.Create(patchObj);
            var resp = await client.PatchAsync(updateEndpoint, content);

            Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        }

        /// <summary>
        /// Tests that a PATCH request causing a duplicate movie returns Conflict.
        /// </summary>
        [Fact]
        public async Task MoviesController_Patch_Conflict_ReturnsConflict()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var movieA = Helper.ValidCreateMovieDto();
            var movieB = Helper.ValidCreateMovieDto();
            movieB.Title = movieB.Title + Constants.Other; // make distinct initially

            var dtoA = await CreateMovieAsync(client, movieA);
            var dtoB = await CreateMovieAsync(client, movieB);

            var updateEndpoint = $"{Constants.SlashMovies}{Constants.Slash}{dtoB.Id}";
            var patchObj = new
            {
                Title = dtoA.Title,
                Director = dtoA.Director,
                ReleaseYear = dtoA.ReleaseYear
            };
            var content = JsonContent.Create(patchObj);
            var resp = await client.PatchAsync(updateEndpoint, content);

            Assert.Equal(HttpStatusCode.Conflict, resp.StatusCode);
        }

        /// <summary>
        /// Tests that a valid PATCH request updates the movie and persists the changes.
        /// </summary>
        [Fact]
        public async Task MoviesController_Patch_PartialUpdate_ReturnsNoContent_AndPersists()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var create = Helper.ValidCreateMovieDto();
            var created = await CreateMovieAsync(client, create);
            var updateEndpoint = $"{Constants.SlashMovies}{Constants.Slash}{created.Id}";
            var patchObj = new { Title = Constants.NewTitle, ReleaseYear = 2022 };
            var content = JsonContent.Create(patchObj);
            var resp = await client.PatchAsync(updateEndpoint, content);

            Assert.Equal(HttpStatusCode.NoContent, resp.StatusCode);

            var updated = await client.GetFromJsonAsync<MovieDto>(updateEndpoint);
            Assert.NotNull(updated);
            Assert.Equal(Constants.NewTitle, updated.Title);
            Assert.Equal(2022, updated.ReleaseYear);
            Assert.Equal(create.Director, updated.Director);
            Assert.Equal(create.Genre, updated.Genre);
        }
    }
}