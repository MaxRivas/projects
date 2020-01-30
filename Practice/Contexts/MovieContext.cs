using System;
using Microsoft.EntityFrameworkCore;
using Practice.Interfaces;

namespace Practice.Contexts
{
    /// <summary>
    /// Class that represents a Movie set from databasae
    /// </summary>
    public class MovieContext : DbContext
    {
        /// <summary>
        /// Default constructor of object
        /// </summary>
        /// <param name="options">
        /// DB movie set
        /// </param>
        /// <returns></returns>
        public MovieContext(DbContextOptions<MovieContext> options) : base(options)
        {

        }

        /// <summary>
        /// Gets and sets the list of movies
        /// </summary>
        /// <value></value>
        public DbSet <IMovie> Movies {get; set;}
    }
}
