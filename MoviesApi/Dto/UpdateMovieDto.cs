using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Dto
{
    public class UpdateMovieDto
    {
        /// <summary>
        /// Identifier of the movie being updated.
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Updated title.
        /// </summary>
        [Required]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Updated director.
        /// </summary>
        [Required]
        public string Director { get; set; } = string.Empty;

        /// <summary>
        /// Updated release year.
        /// </summary>
        [Range(1888, 2100)]
        public int ReleaseYear { get; set; }

        /// <summary>
        /// Updated genre.
        /// </summary>
        [Required]
        public string Genre { get; set; } = string.Empty;
    }
}