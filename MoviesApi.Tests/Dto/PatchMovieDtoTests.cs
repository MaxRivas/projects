using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using MoviesApi.Dto;
using MoviesApi.Tests.Utilities;
using Xunit;

namespace MoviesApi.Dto.Tests
{
    public class PatchMovieDtoTests
    {
        /// <summary>
        /// Tests the validation of an empty PatchMovieDto.
        /// </summary>
        [Fact]
        public void PatchMovieDto_EmptyPatch_PassesValidation()
        {
            var dto = new PatchMovieDto();
            var results = Helper.Validate(dto);
            // No required fields on Patch DTO: validation should pass for empty object
            Assert.Empty(results);
        }

        /// <summary>
        /// Tests the validation of a PatchMovieDto with out-of-range release year.
        /// </summary>
        [Theory]
        [InlineData(1887)]
        [InlineData(2101)]
        public void ReleaseYear_OutOfRange_Fails(int year)
        {
            var dto = new PatchMovieDto { ReleaseYear = year };
            var results = Helper.Validate(dto);
            Assert.NotEmpty(results);
            Assert.Contains(results, r => r.MemberNames != null && r.MemberNames.Contains(nameof(dto.ReleaseYear)));
        }

        /// <summary>
        /// Tests the validation of a PatchMovieDto with boundary release years.
        /// </summary>
        [Theory]
        [InlineData(1888)]
        [InlineData(2100)]
        public void PatchMovieDto_ReleaseYear_Boundaries_Pass(int year)
        {
            var dto = new PatchMovieDto { ReleaseYear = year };
            var results = Helper.Validate(dto);
            Assert.Empty(results);
        }

        /// <summary>
        /// Tests the validation of a PatchMovieDto with a title that is at the maximum length.
        /// </summary>
        /// <param name="length">The length of the title string.</param>
        [Theory]
        [InlineData(256)]
        public void PatchMovieDto_Title_MaxLength_Passes(int length)
        {
            var dto = new PatchMovieDto { Title = Helper.MakeString(length) };
            var results = Helper.Validate(dto);
            Assert.Empty(results);
        }

        /// <summary>
        /// Tests the validation of a PatchMovieDto with a title that exceeds the maximum length.
        /// </summary>
        /// <param name="length">The length of the title string.</param>
        [Theory]
        [InlineData(257)]
        public void PatchMovieDto_Title_TooLong_Passes(int length)
        {
            // Patch DTO does not enforce a max length on Title; long strings are allowed by DataAnnotations
            var dto = new PatchMovieDto { Title = Helper.MakeString(length) };
            var results = Helper.Validate(dto);
            Assert.Empty(results);
        }

        /// <summary>
        /// Tests the validation of a PatchMovieDto with a director name that is at the maximum length.
        /// </summary>  
        /// <param name="length">The length of the director string.</param>
        [Theory]
        [InlineData(256)]
        public void PatchMovieDto_Director_MaxLength_Passes(int length)
        {
            var dto = new PatchMovieDto { Director = Helper.MakeString(length) };
            var results = Helper.Validate(dto);
            Assert.Empty(results);
        }

        /// <summary>
        /// Tests the validation of a PatchMovieDto with a director name that exceeds the maximum length.
        /// </summary>  
        /// <param name="length">The length of the director string.</param>
        [Theory]
        [InlineData(257)]
        public void PatchMovieDto_Director_TooLong_Passes(int length)
        {
            // No max length attribute on Director in the DTO; ensure validation allows long values
            var dto = new PatchMovieDto { Director = Helper.MakeString(length) };
            var results = Helper.Validate(dto);
            Assert.Empty(results);
        }

        /// <summary>
        /// Tests the validation of a PatchMovieDto with a genre that is at the maximum length
        /// </summary>
        /// <param name="length">The length of the genre string.</param>
        [Theory]
        [InlineData(256)]
        public void PatchMovieDto_Genre_MaxLength_Passes(int length)
        {
            var dto = new PatchMovieDto { Genre = Helper.MakeString(length) };
            var results = Helper.Validate(dto);
            Assert.Empty(results);
        }

        /// <summary>
        /// Tests the validation of a PatchMovieDto with a genre that exceeds the maximum length
        /// </summary>
        /// <param name="length">The length of the genre string.</param>
        [Theory]
        [InlineData(257)]
        public void PatchMovieDto_Genre_TooLong_Passes(int length)
        {
            // No max length attribute on Genre in the DTO; ensure validation allows long values
            var dto = new PatchMovieDto { Genre = Helper.MakeString(length) };
            var results = Helper.Validate(dto);
            Assert.Empty(results);
        }

        [Fact]
        public void PatchMovieDto_ReleaseYear_Null_Passes()
        {
            var dto = new PatchMovieDto { ReleaseYear = null };
            var results = Helper.Validate(dto);
            Assert.Empty(results);
        }

        [Fact]
        public void PatchMovieDto_Serialization_Roundtrip()
        {
            var dto = new PatchMovieDto
            {
                Title = "A",
                Director = "B",
                Genre = "C",
                ReleaseYear = 2000
            };
            var json = System.Text.Json.JsonSerializer.Serialize(dto);
            var des = System.Text.Json.JsonSerializer.Deserialize<PatchMovieDto>(json);
            Assert.NotNull(des);
            Assert.Equal(dto.Title, des!.Title);
            Assert.Equal(dto.Director, des.Director);
            Assert.Equal(dto.Genre, des.Genre);
            Assert.Equal(dto.ReleaseYear, des.ReleaseYear);
        }

        /// <summary>
        /// Tests that empty strings for Title, Director, and Genre pass validation.
        /// This ensures that clients can clear these fields if desired.
        /// </summary>
        [Fact]
        public void PatchMovieDto_EmptyStrings_PassValidation()
        {
            var dto = new PatchMovieDto
            {
                Title = string.Empty,
                Director = string.Empty,
                Genre = string.Empty
            };
            var results = Helper.Validate(dto);
            Assert.Empty(results);
        }

        /// <summary>
        /// Tests that whitespace strings for Title, Director, and Genre pass validation.
        /// This ensures that clients can set these fields to whitespace if desired.
        /// </summary>
        [Fact]
        public void PatchMovieDto_WhitespaceStrings_PassValidation()
        {
            var dto = new PatchMovieDto
            {
                Title = "   ",
                Director = " ",
                Genre = "\t"
            };
            var results = Helper.Validate(dto);
            Assert.Empty(results);
        }

        /// <summary>
        /// Tests that all fields set to their maximum lengths with a valid release year pass validation.
        /// </summary>
        [Fact]
        public void PatchMovieDto_AllFields_MaxLength_WithValidReleaseYear_Passes()
        {
            var dto = new PatchMovieDto
            {
                Title = Helper.MakeString(256),
                Director = Helper.MakeString(256),
                Genre = Helper.MakeString(256),
                ReleaseYear = 2000
            };
            var results = Helper.Validate(dto);
            Assert.Empty(results);
        }

        /// <summary>
        /// Tests that all fields set to null pass validation.
        /// </summary>
        [Fact]
        public void PatchMovieDto_NullStrings_PassValidation()
        {
            var dto = new PatchMovieDto
            {
                Title = null,
                Director = null,
                Genre = null,
                ReleaseYear = null
            };
            var results = Helper.Validate(dto);
            Assert.Empty(results);
        }
        
        /// <summary>
        /// Tests that serialization and deserialization of a PatchMovieDto with all null fields works correctly.
        /// </summary>
        [Fact]
        public void Serialization_WithNulls_Roundtrip()
        {
            var dto = new PatchMovieDto
            {
                Title = null,
                Director = null,
                Genre = null,
                ReleaseYear = null
            };
            var json = JsonSerializer.Serialize(dto, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var des = JsonSerializer.Deserialize<PatchMovieDto>(json);
            Assert.NotNull(des);
            Assert.Null(des!.Title);
            Assert.Null(des.Director);
            Assert.Null(des.Genre);
            Assert.Null(des.ReleaseYear);
        }
    }
}
