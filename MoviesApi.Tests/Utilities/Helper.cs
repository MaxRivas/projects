using System;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoviesApi.Models;
using MoviesApi.Dto;
using Xunit;
using MoviesApi.Tests.Utilities;
using static System.Net.HttpStatusCode;

namespace MoviesApi.Tests.Utilities
{
    internal static class Helper
    {
        /// <summary>
        /// Creates a valid UpdateMovieDto instance for testing.
        /// </summary>
        /// <returns></returns>
        public static UpdateMovieDto ValidUpdateMovieDto()
        {
            return new UpdateMovieDto
            {
                Id = 9999,
                Title = Constants.TestMovieTitle,
                Director = Constants.TestMovieDirector,
                ReleaseYear = Constants.TestMovieReleaseYear,
                Genre = Constants.TestMovieGenre
            };
        }

        /// <summary>
        /// Creates a valid CreateMovieDto instance for testing.
        /// </summary>
        /// <returns></returns>
        public static CreateMovieDto ValidCreateMovieDto()
        {
            return new CreateMovieDto
            {
                Title = Constants.TestMovieTitle,
                Director = Constants.TestMovieDirector,
                ReleaseYear = Constants.TestMovieReleaseYear,
                Genre = Constants.TestMovieGenre
            };
        }
    }
}
