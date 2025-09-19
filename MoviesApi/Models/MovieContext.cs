using Microsoft.EntityFrameworkCore;

namespace MoviesApi.Models
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options)
            : base(options)
        {
        }

        public DbSet<MovieContext> MovieItems { get; set; } = null!;
    }
}