using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Models;
using MoviesApi.Controllers;
using Xunit;
using MoviesApi.Tests.Utilities;
using System.Net;

namespace MoviesApi.Controllers
{
    public partial class MoviesControllerTests
    {
        /// <summary>
        /// Tests that deleting an existing movie via the controller returns NoContent.
        /// </summary>
        [Fact]
        public async Task Delete_Controller_NoContent()
        {
            // Arrange: create a fresh in-memory context and seed a movie
            var options = new DbContextOptionsBuilder<MovieContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            await using (var ctx = new MovieContext(options))
            {
                var movie = new Movie { Title = "DelTest", Director = "D", ReleaseYear = 2000, Genre = "Drama" };
                ctx.MovieItems.Add(movie);
                await ctx.SaveChangesAsync();

                var controller = new MoviesController(ctx);

                // Act: delete the existing movie
                var result = await controller.Delete(movie.Id);

                // Assert: should be NoContent
                Assert.IsType<Microsoft.AspNetCore.Mvc.NoContentResult>(result);
            }
        }

        /// <summary>
        /// Tests that deleting a non-existent movie via the controller returns NotFound.
        /// </summary>
        [Fact]
        public async Task Delete_Controller_NotFound()
        {
            // Arrange: use a fresh in-memory context with no movies
            var options = new DbContextOptionsBuilder<MovieContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            await using (var ctx = new MovieContext(options))
            {
                var controller = new MoviesController(ctx);

                // Act: attempt to delete a non-existent movie id
                var notFound = await controller.Delete(99999);

                // Assert: should be NotFound
                Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundResult>(notFound);
            }
        }

        /// <summary>
        /// Tests update of a non-existent movie returning NotFound.
        /// </summary>
        [Fact]
        public async Task Delete_Succeeds()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var create = Helper.ValidCreateMovieDto();
            var created = await CreateMovieAsync(client, create);

            var updateEndpoint = $"{Constants.SlashMovies}{Constants.Slash}{created.Id}";
            var del = await client.DeleteAsync(updateEndpoint);
            Assert.Equal(HttpStatusCode.NoContent, del.StatusCode);

            var get = await client.GetAsync(updateEndpoint);
            Assert.Equal(HttpStatusCode.NotFound, get.StatusCode);
        }

        /// <summary>
        /// Tests deletion of a non-existent movie.
        /// </summary>
        [Fact]
        public async Task Delete_NotFound_ReturnsNotFound()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var resp = await client.DeleteAsync($"{Constants.SlashMovies}{Constants.Slash}{Constants.ReleaseYearMax}");
            Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
        }
    }
}
