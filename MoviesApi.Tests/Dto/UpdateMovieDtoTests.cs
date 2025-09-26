using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MoviesApi.Dto;
using Xunit;
using MoviesApi.Tests.Utilities;

namespace MoviesApi.Dto.Tests
{
    public class UpdateMovieDtoTest
    {
        /// <summary>
        /// Tests that a valid DTO passes validation.
        /// </summary>
        [Fact]
        public void ValidDto_PassesValidation()
        {
            var dto = Helper.ValidUpdateMovieDto();
            var results = Helper.Validate(dto);
            Assert.Empty(results);
        }

        /// <summary>
        /// Tests that Title null or empty should produce a validation error for Title.
        /// </summary>
        /// <param name="titleValue"></param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Title_NullOrEmpty_Fails(string? titleValue)
        {
            var dto = Helper.ValidUpdateMovieDto();
            dto.Title = titleValue ?? string.Empty;
            var results = Helper.Validate(dto);
            Assert.NotEmpty(results);
            Assert.Contains(results, r => r.MemberNames != null && r.MemberNames.Contains(nameof(dto.Title)));
        }

        /// <summary>
        ///  Tests that Director null or empty should produce a validation error for Director.
        /// </summary>
        /// <param name="directorValue"></param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Director_NullOrEmpty_Fails(string? directorValue)
        {
            var dto = Helper.ValidUpdateMovieDto();
            dto.Director = directorValue ?? string.Empty;
            var results = Helper.Validate(dto);
            Assert.NotEmpty(results);
            Assert.Contains(results, r => r.MemberNames != null && r.MemberNames.Contains(nameof(dto.Director)));
        }
        // Genre_NullOrEmpty_Fails: Genre null or empty should produce a validation error for Genre.
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Genre_NullOrEmpty_Fails(string? genreValue)
        {
            var dto = Helper.ValidUpdateMovieDto();
            dto.Genre = genreValue ?? string.Empty;
            var results = Helper.Validate(dto);
            Assert.NotEmpty(results);
            Assert.Contains(results, r => r.MemberNames != null && r.MemberNames.Contains(nameof(dto.Genre)));
        }

        /// <summary>
        /// Tests that release years below allowed minimum should fail validation.
        /// </summary>
        [Fact]
        public void ReleaseYear_BelowMin_Fails()
        {
            var dto = Helper.ValidUpdateMovieDto();
            dto.ReleaseYear = 1800;
            var results = Helper.Validate(dto);
            Assert.NotEmpty(results);
            Assert.Contains(results, r => r.MemberNames != null && r.MemberNames.Contains(nameof(dto.ReleaseYear)));
        }

        /// <summary>
        /// Tests that release years above allowed maximum should fail validation.
        /// </summary>
        [Fact]
        public void ReleaseYear_AboveMax_Fails()
        {
            var dto = Helper.ValidUpdateMovieDto();
            dto.ReleaseYear = 2200;
            var results = Helper.Validate(dto);
            Assert.NotEmpty(results);
            Assert.Contains(results, r => r.MemberNames != null && r.MemberNames.Contains(nameof(dto.ReleaseYear)));
        }

        /// <summary>
        /// Tests that valid release year boundaries pass validation.  
        /// </summary>
        /// <param name="year"></param>
        [Theory]
        [InlineData(1888)]
        [InlineData(2100)]
        public void ReleaseYear_Boundaries_Pass(int year)
        {
            var dto = Helper.ValidUpdateMovieDto();
            dto.ReleaseYear = year;
            var results = Helper.Validate(dto);
            Assert.Empty(results);
        }
    }
}