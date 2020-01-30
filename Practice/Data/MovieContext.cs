using Microsoft.EntityFrameworkCore;
using Practice.Interfaces;

namespace Practice.Data
{
    /// <summary>
    /// Class that represents the DB movie contet
    /// </summary>
    public class MovieContext : DbContext
    {
        /// <summary>
        /// Default constructor of class
        /// </summary>
        /// <param name="options">
        /// List of options to initialize</param>
        /// <returns></returns>
        public MovieContext(DbContextOptions<MovieContext> options) : base(options)
        {
        }

        /// <summary>
        /// Gets and sets the list of movies DB set
        /// </summary>
        /// <value>
        /// List of Movies
        /// </value>
        public DbSet <Movie> Movies {get; set;}
    }
}
