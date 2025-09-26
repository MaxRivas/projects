using System.ComponentModel.DataAnnotations;
using System.Linq;
using MoviesApi.Dto;
using MoviesApi.Tests.Utilities;
using Xunit;

namespace MoviesApi.Dto.Tests
{
    public class PatchMovieDtoTests
    {
        [Fact]
        public void EmptyPatch_PassesValidation()
        {
            var dto = new PatchMovieDto();
            var results = Helper.Validate(dto);
            // No required fields on Patch DTO: validation should pass for empty object
            Assert.Empty(results);
        }

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

        [Theory]
        [InlineData(1888)]
        [InlineData(2100)]
        public void ReleaseYear_Boundaries_Pass(int year)
        {
            var dto = new PatchMovieDto { ReleaseYear = year };
            var results = Helper.Validate(dto);
            Assert.Empty(results);
        }
    }
}
