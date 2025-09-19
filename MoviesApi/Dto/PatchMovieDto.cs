using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Dto;

/// <summary>
/// DTO for partial updates (PATCH). All properties are nullable to indicate optional presence.
/// </summary>
public class PatchMovieDto
{
    public string? Title { get; set; }

    public string? Director { get; set; }

    [Range(1888, 2100)]
    public int? ReleaseYear { get; set; }

    public string? Genre { get; set; }
}