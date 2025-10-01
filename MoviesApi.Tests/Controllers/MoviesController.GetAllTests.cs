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
        // ...existing code... (shared helpers are defined in MoviesController.Helpers.cs)

        /// <summary>
        /// Tests that GetAll returns an empty list when there are no movies.
        /// </summary>
        /// <returns>Empty list of MovieDto</returns>
        [Fact]
        public async Task GetAll_ReturnsEmptyList()
        {
            await using var ctx = CreateContext();
            var controller = CreateController(ctx);

            var result = await controller.GetAll();
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        /// <summary>
        /// Tests that GetAll returns all movies in the database.
        /// </summary>
        /// <returns>List of all MovieDto</returns>
        [Fact]
        public async Task GetAll_ReturnsAllMovies()
        {
            await using var ctx = CreateContext();
            var movie1 = new Movie { Title = "GetAll1", Director = "D1", ReleaseYear = 2001, Genre = "Drama" };
            var movie2 = new Movie { Title = "GetAll2", Director = "D2", ReleaseYear = 2002, Genre = "Action" };
            ctx.MovieItems.AddRange(movie1, movie2);
            await ctx.SaveChangesAsync();

            var controller = CreateController(ctx);
            var result = await controller.GetAll();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.Title == movie1.Title && r.Director == movie1.Director && r.ReleaseYear == movie1.ReleaseYear && r.Genre == movie1.Genre);
            Assert.Contains(result, r => r.Title == movie2.Title && r.Director == movie2.Director && r.ReleaseYear == movie2.ReleaseYear && r.Genre == movie2.Genre);
        }

        /// <summary>
        /// Tests that GetAll maps Movie entities to MovieDto correctly.
        /// </summary>
        [Fact]
        public async Task GetAll_MapsToMovieDtoProperly()
        {
            // Arrange
            await using var ctx = CreateContext();
            var movie = new Movie { Title = "MapTest", Director = "MapDir", ReleaseYear = 1999, Genre = "Comedy" };
            ctx.MovieItems.Add(movie);
            await ctx.SaveChangesAsync();

            var controller = CreateController(ctx);

            // Act
            var result = await controller.GetAll();

            // Assert
            Assert.Single(result);
            var dto = result.First();
            Assert.True(dto.Id > 0);
            Assert.Equal(movie.Title, dto.Title);
            Assert.Equal(movie.Director, dto.Director);
            Assert.Equal(movie.ReleaseYear, dto.ReleaseYear);
            Assert.Equal(movie.Genre, dto.Genre);
        }        

        /// <summary>
        /// Tests retrieval of all movies returning the inserted movies.
        /// </summary>
        [Fact]
        public async Task GetAll_ReturnsInsertedMovies()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var movie1 = Helper.ValidCreateMovieDto();
            var movie2 = Helper.ValidCreateMovieDto();

            // Ensure movie2 differs from movie1 to avoid duplicate detection
            movie2.Title = movie1.Title + "_2";

            var respA = await client.PostAsJsonAsync(Constants.SlashMovies, movie1);
            var respB = await client.PostAsJsonAsync(Constants.SlashMovies, movie2);

            Assert.Equal(HttpStatusCode.Created, respA.StatusCode);
            Assert.Equal(HttpStatusCode.Created, respB.StatusCode);

            // Create two more distinct movies via helper to ensure list contains multiple entries
            var helperA = Helper.ValidCreateMovieDto();
            const string helperTitle = "HelperMovie_";
            helperA.Title = helperTitle + Guid.NewGuid().ToString("N");
            var helperB = Helper.ValidCreateMovieDto();
            helperB.Title = helperTitle + Guid.NewGuid().ToString("N");
            await CreateMovieAsync(client, helperA);
            await CreateMovieAsync(client, helperB);

            var list = await client.GetFromJsonAsync<MovieDto[]>(Constants.SlashMovies);
            Assert.NotNull(list);
            Assert.True(list.Length >= 2);
            Assert.Contains(list, m => m.Title == movie1.Title);
            Assert.Contains(list, m => m.Title == movie2.Title);
        }
    }
}