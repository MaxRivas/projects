using System;
using Microsoft.EntityFrameworkCore;
using Practice.Interfaces;

namespace Practice.Contexts
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options) : base(options)
        {

        }

        public DbSet <IMovie> Movies {get; set;}
    }
}
