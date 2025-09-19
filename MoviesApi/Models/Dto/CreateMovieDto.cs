using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Models.Dto;

public class CreateMovieDto
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Director { get; set; } = string.Empty;

    [Range(1888, 2100)]
    public int ReleaseYear { get; set; }

    public string Genre { get; set; } = string.Empty;
}
