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
        /// Tests retrieval of a movie by ID returning NotFound for non-existent ID.
        /// </summary>
        [Fact]
        public async Task MoviesController_GetById_NotFound()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var resp = await client.GetAsync($"{Constants.SlashMovies}{Constants.Slash}{Constants.ReleaseYearMax}");
            Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
        }

        /// <summary>
        /// Tests retrieval of a movie by ID and verifies the returned data matches the created movie.
        /// </summary>
        [Fact]
        public async Task MoviesController_GetById_Succeeds()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var create = Helper.ValidCreateMovieDto();
            var created = await CreateMovieAsync(client, create);

            var resp = await client.GetAsync($"{Constants.SlashMovies}{Constants.Slash}{created.Id}");
            Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

            var dto = await resp.Content.ReadFromJsonAsync<MovieDto>();
            Assert.NotNull(dto);
            Assert.Equal(created.Id, dto.Id);
            Assert.Equal(create.Title, dto.Title);
            Assert.Equal(create.Director, dto.Director);
            Assert.Equal(create.ReleaseYear, dto.ReleaseYear);
            Assert.Equal(create.Genre, dto.Genre);
        }
    }
}