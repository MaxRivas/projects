namespace MoviesApi.Models.Dto;

public class MovieDto
{
    /// <summary>
    /// Movie identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Movie title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Movie director.
    /// </summary>
    public string Director { get; set; } = string.Empty;

    /// <summary>
    /// Release year.
    /// </summary>
    public int ReleaseYear { get; set; }

    /// <summary>
    /// Movie genre.
    /// </summary>
    public string Genre { get; set; } = string.Empty;
}
