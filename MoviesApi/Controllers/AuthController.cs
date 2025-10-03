using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MoviesApi.Dto;
using MoviesApi.Models;

namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration config, ILogger<AuthController> logger)
        {
            _config = config;
            _logger = logger;
        }

        // allow anonymous so clients can obtain a token
        [AllowAnonymous]
        [HttpPost("token")]
        public IActionResult Token([FromBody] LoginDto dto)
        {
            if (dto == null)
                return BadRequest(new { error = "Missing body" });

            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { error = "username and password are required" });

            // Simple dev credential check — replace with real user validation in production
            if (dto.Username != "dev" || dto.Password != "dev")
            {
                _logger.LogInformation("Invalid login attempt for user '{User}'", dto.Username);
                return Unauthorized(new { error = "Invalid username or password" });
            }

            // Build token using configuration values (with safe defaults for local development)
            var jwtKey = _config["Jwt:Key"] ?? "ChangeThisDevSigningKey_ReplaceInProd_UseAtLeast32Chars!";
            var jwtIssuer = _config["Jwt:Issuer"] ?? "MoviesApi";
            var expiresMinutes = int.TryParse(_config["Jwt:ExpireMinutes"], out var m) ? m : 60;

            // Ensure key material is at least 256 bits (32 bytes) for HS256.
            var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
            if (keyBytes.Length < 32)
            {
                // Derive a 256-bit key deterministically from the provided secret
                // (safe for dev; in production supply a high-entropy key of sufficient length).
                using var sha = SHA256.Create();
                keyBytes = sha.ComputeHash(keyBytes);
            }
            var key = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, dto.Username),
                new Claim(ClaimTypes.Name, dto.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
                signingCredentials: creds);

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                access_token = tokenStr,
                token_type = "Bearer",
                expires_in = expiresMinutes * 60
            });
        }
    }
}