using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Dto
{
    /// <summary>
    /// DTO for login requests containing username and password.
    /// </summary>
    public class LoginDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}