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
    public partial class MoviesControllerTests
    {
        /// <summary>
        /// Tests the creation of a movie.
        /// </summary>
        [Fact]
        public async Task MoviesController_Create_ReturnsCreatedAndLocation()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var create = Helper.ValidCreateMovieDto();

            var resp = await client.PostAsJsonAsync(Constants.SlashMovies, create);

            Assert.Equal(HttpStatusCode.Created, resp.StatusCode);
            Assert.NotNull(resp.Headers.Location);
            var dto = await resp.Content.ReadFromJsonAsync<MovieDto>();
            Assert.NotNull(dto);
            Assert.True(dto.Id > 0);
            Assert.Equal(create.Title, dto.Title);
        }

        /// <summary>
        /// Tests the creation of a duplicate movie returning Conflict.
        /// </summary>
        [Fact]
        public async Task MoviesController_Create_Duplicate_ReturnsConflict()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var create = Helper.ValidCreateMovieDto();

            var first = await client.PostAsJsonAsync(Constants.SlashMovies, create);
            Assert.Equal(HttpStatusCode.Created, first.StatusCode);

            var second = await client.PostAsJsonAsync(Constants.SlashMovies, create);
            Assert.Equal(HttpStatusCode.Conflict, second.StatusCode);
        }

        /// <summary>
        /// Tests the creation of a movie and verifies it is persisted.
        /// </summary>
        [Fact]
        public async Task MoviesController_Create_Success_ReturnsCreatedAndPersists()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var create = Helper.ValidCreateMovieDto();

            var result = await client.PostAsJsonAsync(Constants.SlashMovies, create);
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            var dto = await result.Content.ReadFromJsonAsync<MovieDto>();
            Assert.NotNull(dto);
            Assert.True(dto.Id > 0);
            Assert.Equal(create.Title, dto.Title);
            Assert.Equal(create.Director, dto.Director);
            Assert.Equal(create.ReleaseYear, dto.ReleaseYear);
            Assert.Equal(create.Genre, dto.Genre);

            var persisted = await client.GetFromJsonAsync<MovieDto>($"{Constants.SlashMovies}{Constants.Slash}{dto.Id}");
            Assert.NotNull(persisted);
            Assert.Equal(create.Title, persisted.Title);
            Assert.Equal(create.Director, persisted.Director);
            Assert.Equal(create.ReleaseYear, persisted.ReleaseYear);
            Assert.Equal(create.Genre, persisted.Genre);
        }

        /// <summary>
        /// Tests the creation of a movie with invalid genre data returning BadRequest.
        /// </summary>
        [Fact]
        public async Task MoviesController_Create_InvalidGenre_ReturnsBadRequest()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var create = Helper.ValidCreateMovieDto();
            create.Genre = string.Empty;

            var resp = await client.PostAsJsonAsync(Constants.SlashMovies, create);

            Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        }

        /// <summary>
        /// Tests the creation of a movie with invalid title data returning BadRequest.
        /// </summary>
        [Fact]
        public async Task MoviesController_Create_InvalidTitle_ReturnsBadRequest()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var create = Helper.ValidCreateMovieDto();
            create.Title = string.Empty;

            var resp = await client.PostAsJsonAsync(Constants.SlashMovies, create);

            Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        }

        /// <summary>
        /// Tests the creation of a movie with invalid director data returning BadRequest.
        /// </summary>
        [Fact]
        public async Task MoviesController_Create_InvalidDirector_ReturnsBadRequest()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var create = Helper.ValidCreateMovieDto();
            create.Director = string.Empty;

            var resp = await client.PostAsJsonAsync(Constants.SlashMovies, create);

            Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        }

        /// <summary>
        /// Tests the creation of a movie with invalid release date data returning BadRequest.
        /// </summary>
        [Fact]
        public async Task MoviesController_Create_InvalidReleaseDate_ReturnsBadRequest()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var create = Helper.ValidCreateMovieDto();
            create.ReleaseYear = -1800;

            var resp = await client.PostAsJsonAsync(Constants.SlashMovies, create);

            Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        }

        /// <summary>
        /// Tests the creation of a movie with invalid release date data returning BadRequest.
        /// </summary>
        [Fact]
        public async Task MoviesController_Create_InvalidParameters_ReturnsBadRequest()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            // Build an intentionally invalid payload (missing required fields / out-of-range year)
            var create = new CreateMovieDto { Title = string.Empty, Director = string.Empty, ReleaseYear = -1800, Genre = string.Empty };

            var resp = await client.PostAsJsonAsync(Constants.SlashMovies, create);

            Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        }

        /// <summary>
        /// Tests creation of a case-insensitive duplicate movie.
        /// </summary>
        [Fact]
        public async Task MoviesController_Create_CaseInsensitiveDuplicate_ReturnsConflict()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var baseCreate = Helper.ValidCreateMovieDto();

            const string updatedTitle = "CaseTest";
            const string updatedDirector = "DiRecTor";
            baseCreate.Title = updatedTitle;
            baseCreate.Director = updatedDirector;

            var first = await client.PostAsJsonAsync(Constants.SlashMovies, baseCreate);
            Assert.Equal(HttpStatusCode.Created, first.StatusCode);

            // Attempt duplicate with different casing
            var duplicate = Helper.ValidCreateMovieDto();
            duplicate.Title = updatedTitle.ToLower(); // different case
            duplicate.Director = updatedDirector.ToLower(); // different case
            duplicate.ReleaseYear = baseCreate.ReleaseYear; // same year
            duplicate.Genre = baseCreate.Genre;

            var second = await client.PostAsJsonAsync(Constants.SlashMovies, duplicate);
            Assert.Equal(HttpStatusCode.Conflict, second.StatusCode);
        }
    }
}