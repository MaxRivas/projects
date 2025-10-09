using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MoviesApi.Data;

#nullable disable

namespace MoviesApi.Migrations
{
    [DbContext(typeof(MoviesDbContext))]
    partial class MoviesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("MoviesApi.Data.Movie", b =>
            {
                b.Property<int>("Id");
                b.Property<string>("Title");
                b.Property<string>("Genre");
                b.Property<DateTime>("ReleaseDate");
                b.Property<string>("Director");
                b.Property<DateTime>("CreatedAt");
                b.HasKey("Id");
                b.ToTable("Movies");
            });
        }
    }
}
