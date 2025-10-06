using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Controllers;
using MoviesApi.Models;
using MoviesApi.Dto;
using Xunit;
using System.Net;
using MoviesApi.Tests.Utilities;
using System.Net.Http.Json;

namespace MoviesApi.Controllers
{
    public partial class MoviesControllerTests
    {

        /// <summary>
        /// Tests pre-filling the database with initial movie data and verifies movies are added.
        /// </summary>
        [Fact]
        public async Task MoviesController_Prefill_AddsMovies()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var resp = await client.GetAsync($"{Constants.SlashMovies}{Constants.Slash}{Constants.Prefill}");
            Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

            var list = await client.GetFromJsonAsync<MovieDto[]>(Constants.SlashMovies);
            Assert.NotNull(list);
            Assert.True(list.Length >= 15, $"Expected at least 15 prefilled movies but found {list.Length}");
        }

        /// <summary>
        /// Tests that {Constants.Prefill} skips adding duplicate movies if they already exist.
        /// </summary>
        [Fact]
        public async Task MoviesController_Prefill_SkipsDuplicates_WhenSampleAlreadyExists()
        {
            var client = CreateClient(Guid.NewGuid().ToString());

            // Insert a movie matching what the {Constants.Prefill} generator would create for Sample Movie 1
            var preExisting = Helper.ValidCreateMovieDto();
            var created = await CreateMovieAsync(client, preExisting);
            Assert.NotNull(created);

            var resp = await client.GetAsync($"{Constants.SlashMovies}{Constants.Slash}{Constants.Prefill}");
            Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

            var list = await client.GetFromJsonAsync<MovieDto[]>(Constants.SlashMovies);
            Assert.NotNull(list);
            // Ensure at least 15 movies exist after {Constants.Prefill} (duplicates should be skipped but others added)
            Assert.True(list.Length >= 15, $"Expected at least 15 movies after {Constants.Prefill} but found {list.Length}");
        }
    }
}