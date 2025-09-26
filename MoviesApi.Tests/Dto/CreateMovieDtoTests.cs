using System.ComponentModel.DataAnnotations;
using System.Linq;
using MoviesApi.Dto;
using MoviesApi.Tests.Utilities;
using Xunit;

namespace MoviesApi.Dto.Tests
{
    public class CreateMovieDtoTests
    {
        /// <summary>
        /// Tests the validation of a valid CreateMovieDto.
        /// </summary>
        [Fact]
        public void ValidDto_PassesValidation()
        {
            var dto = Helper.ValidCreateMovieDto();
            var results = Helper.Validate(dto);
            Assert.Empty(results);
        }

        /// <summary>
        /// Tests the validation of a CreateMovieDto with null or empty title.
        /// </summary>
        /// <param name="value">string value to test</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Title_NullOrEmpty_Fails(string? value)
        {
            var dto = Helper.ValidCreateMovieDto();
            dto.Title = value ?? string.Empty;
            var results = Helper.Validate(dto);
            Assert.NotEmpty(results);
            Assert.Contains(results, r => r.MemberNames != null && r.MemberNames.Contains(nameof(dto.Title)));
        }

        /// <summary>
        /// Tests the validation of a CreateMovieDto with null or empty director.
        /// </summary>
        /// <param name="value">string value to test</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Director_NullOrEmpty_Fails(string? value)
        {
            var dto = Helper.ValidCreateMovieDto();
            dto.Director = value ?? string.Empty;
            var results = Helper.Validate(dto);
            Assert.NotEmpty(results);
            Assert.Contains(results, r => r.MemberNames != null && r.MemberNames.Contains(nameof(dto.Director)));
        }

        /// <summary>
        /// Tests the validation of a CreateMovieDto with out-of-range release year.
        /// </summary>
        /// <param name="year">int year to test</param>
        [Theory]
        [InlineData(1887)]
        [InlineData(2101)]
        public void ReleaseYear_OutOfRange_Fails(int year)
        {
            var dto = Helper.ValidCreateMovieDto();
            dto.ReleaseYear = year;
            var results = Helper.Validate(dto);
            Assert.NotEmpty(results);
            Assert.Contains(results, r => r.MemberNames != null && r.MemberNames.Contains(nameof(dto.ReleaseYear)));
        }

        /// <summary>
        /// Tests the validation of a CreateMovieDto with boundary release years.
        /// </summary>
        /// <param name="year">int year to test</param>
        [Theory]
        [InlineData(1888)]
        [InlineData(2100)]
        public void ReleaseYear_Boundaries_Pass(int year)
        {
            var dto = Helper.ValidCreateMovieDto();
            dto.ReleaseYear = year;
            var results = Helper.Validate(dto);
            Assert.Empty(results);
        }

        /// <summary>
        /// Tests the validation of a CreateMovieDto with null or empty genre.
        /// </summary>  
        /// <param name="value">string value to test</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Genre_NullOrEmpty_Fails(string? value)
        {
            var dto = Helper.ValidCreateMovieDto();
            dto.Genre = value ?? string.Empty;
            var results = Helper.Validate(dto);
            Assert.NotEmpty(results);
            Assert.Contains(results, r => r.MemberNames != null && r.MemberNames.Contains(nameof(dto.Genre)));
        }
    }
}
