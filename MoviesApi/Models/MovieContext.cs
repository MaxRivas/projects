using Microsoft.EntityFrameworkCore;

namespace MoviesApi.Models
{
    /// <summary>
    /// Entity Framework Core context for movie entities.
    /// </summary>
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// DbSet of <see cref="Movie"/> entities.
        /// </summary>
        // Expose the Movie entity set (was incorrectly typed as DbSet<MovieContext>)
        public DbSet<Movie> MovieItems { get; set; } = null!;
    }
}