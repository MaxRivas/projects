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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Tests.Utilities
{
    internal static class Helper
    {
        /// <summary>
        /// Creates a valid UpdateMovieDto instance for testing.
        /// </summary>
        /// <returns>Valid UpdateMovieDto instance</returns>
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
        /// <returns>Valid CreateMovieDto instance</returns>
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

        /// <summary>
        /// Validates the given model using DataAnnotations and returns validation results.
        /// </summary>
        /// <param name="model">model to validate</param>
        /// <returns>List of validation results</returns>
        internal static IList<ValidationResult> Validate(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, serviceProvider: null, items: null);
            Validator.TryValidateObject(model, context, results, validateAllProperties: true);
            return results;
        }

        /// <summary>
        /// Generates a string of specified length consisting of repeated 'x' characters.
        /// </summary>
        /// <param name="length">The length of the string to generate.</param>
        /// <returns>A string of the specified length.</returns>
        internal static string MakeString(int length) => new(Constants.N.ToCharArray()[0], length);

    }
}
