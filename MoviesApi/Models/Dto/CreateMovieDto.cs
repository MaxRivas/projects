using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Models.Dto;

public class CreateMovieDto
{
    /// <summary>
    /// Title of the movie. Required.
    /// </summary>
    [Required]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Director of the movie. Required.
    /// </summary>
    [Required]
    public string Director { get; set; } = string.Empty;

    /// <summary>
    /// Year the movie was released. Must be between 1888 and 2100.
    /// </summary>
    [Range(1888, 2100)]
    public int ReleaseYear { get; set; }

    /// <summary>
    /// Genre of the movie.
    /// </summary>
    public string Genre { get; set; } = string.Empty;
}
