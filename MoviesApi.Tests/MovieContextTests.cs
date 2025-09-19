using System.Linq;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Models;
using Xunit;

namespace MoviesApi.Tests
{
    /// <summary>
    /// Unit tests for <see cref="MoviesApi.Models.MovieContext"/> using the in-memory provider.
    /// These tests validate basic CRUD behavior of the EF Core context without involving HTTP.
    /// </summary>
    public class MovieContextTests
    {
        /// <summary>
        /// Verifies a <see cref="Movie"/> can be added to and read back from the in-memory database.
        /// Steps:
        /// 1. Create DbContextOptions using the InMemory provider.
        /// 2. Insert a movie and save changes.
        /// 3. Open a new context with the same options and assert the movie exists with expected values.
        /// </summary>
        [Fact]
        public void CanAddAndReadMovie_InMemory()
        {
            var options = new DbContextOptionsBuilder<MovieContext>()
                .UseInMemoryDatabase(databaseName: "Test_MoviesDb")
                .Options;

            // Insert a movie into the in-memory store
            using (var context = new MovieContext(options))
            {
                context.MovieItems.Add(new Movie { Title = "The Test Movie", Director = "Jane Developer", ReleaseYear = 2025, Genre = "Test" });
                context.SaveChanges();
            }

            // Read the movie back using a separate context instance to simulate a different request/session
            using (var context = new MovieContext(options))
            {
                var movie = context.MovieItems.FirstOrDefault(m => m.Title == "The Test Movie");
                Assert.NotNull(movie);
                Assert.Equal("Jane Developer", movie!.Director);
                Assert.Equal(2025, movie.ReleaseYear);
            }
        }
    }
}