using Microsoft.EntityFrameworkCore;

namespace MoviesApi.Models
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options)
            : base(options)
        {
        }

        // Expose the Movie entity set (was incorrectly typed as DbSet<MovieContext>)
        public DbSet<Movie> MovieItems { get; set; } = null!;
    }
}