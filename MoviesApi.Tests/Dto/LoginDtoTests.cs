using System.Linq;
using MoviesApi.Dto;
using MoviesApi.Tests.Utilities;
using Xunit;

namespace MoviesApi.Dto.Tests
{
    public class LoginDtoTests
    {
        /// <summary>
        /// Tests the validation of a LoginDto with missing required fields.
        /// </summary>
        [Fact]
        public void LoginDto_MissingFields_FailsValidation()
        {
            var dto = new LoginDto(); // Username and Password empty by default
            var results = Helper.Validate(dto);
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(LoginDto.Username)));
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(LoginDto.Password)));
        }

        /// <summary>
        /// Tests the validation of a valid LoginDto.
        /// </summary>
        [Fact]
        public void LoginDto_ValidDto_PassesValidation()
        {
            var dto = new LoginDto { Username = Constants.Dev, Password = Constants.Dev };
            var results = Helper.Validate(dto);
            Assert.Empty(results);
        }
    }
}