using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using MoviesApi.Controllers;
using MoviesApi.Dto;
using Xunit;

namespace MoviesApi.Controllers.Tests
{
    public class AuthControllerTests
    {
        /// <summary>
        /// Creates an instance of AuthController with optional in-memory configuration settings.
        /// </summary>
        /// <param name="inMemorySettings"></param>
        /// <returns></returns>
        private AuthController CreateController(Dictionary<string, string>? inMemorySettings = null)
        {
            var configBuilder = new ConfigurationBuilder();
            if (inMemorySettings != null)
                configBuilder.AddInMemoryCollection(inMemorySettings);
            var config = configBuilder.Build();
            var logger = new NullLogger<AuthController>();
            return new AuthController(config, logger);
        }

        /// <summary>
        /// Tests that valid dev credentials return an OK result with an access token.
        /// </summary>
        [Fact]
        public void Token_Returns_Ok_With_AccessToken_For_Valid_Dev_Credentials()
        {
            var ctrl = CreateController();
            var dto = new LoginDto { Username = "dev", Password = "dev" };

            var result = ctrl.Token(dto);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(ok.Value);

            var accessTokenProp = ok.Value!.GetType().GetProperty("access_token");
            Assert.NotNull(accessTokenProp);

            var token = accessTokenProp.GetValue(ok.Value) as string;
            Assert.False(string.IsNullOrEmpty(token));
        }

        /// <summary>
        /// Tests that invalid credentials return an Unauthorized result with an error message. 
        /// </summary>
        [Fact]
        public void Token_Returns_Unauthorized_For_Invalid_Credentials()
        {
            var ctrl = CreateController();
            var dto = new LoginDto { Username = "bad", Password = "bad" };

            var result = ctrl.Token(dto);

            var unauth = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.NotNull(unauth.Value);

            // optional: server returns { error = "..." } — verify shape contains error
            var errorProp = unauth.Value!.GetType().GetProperty("error");
            Assert.NotNull(errorProp);
            var errorVal = errorProp.GetValue(unauth.Value) as string;
            Assert.False(string.IsNullOrEmpty(errorVal));
        }
    }
}