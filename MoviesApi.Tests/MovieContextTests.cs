using System.Linq;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Models;
using Xunit;

namespace MoviesApi.Tests;

public class MovieContextTests
{
    [Fact]
    public void CanAddAndReadMovie_InMemory()
    {
        var options = new DbContextOptionsBuilder<MovieContext>()
            .UseInMemoryDatabase(databaseName: "Test_MoviesDb")
            .Options;

        // Insert a movie
        using (var context = new MovieContext(options))
        {
            context.MovieItems.Add(new Movie { Title = "The Test Movie", Director = "Jane Developer", ReleaseYear = 2025, Genre = "Test" });
            context.SaveChanges();
        }

        // Read the movie back
        using (var context = new MovieContext(options))
        {
            var movie = context.MovieItems.FirstOrDefault(m => m.Title == "The Test Movie");
            Assert.NotNull(movie);
            Assert.Equal("Jane Developer", movie!.Director);
            Assert.Equal(2025, movie.ReleaseYear);
        }
    }
}