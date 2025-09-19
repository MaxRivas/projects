namespace MoviesApi.Models
{
    /// <summary>
    /// Represents a movie entity stored in the application's database.
    /// </summary>
    public class Movie
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Movie"/> class and ensures non-null strings.
        /// </summary>
        public Movie()
        {
            this.Title = string.Empty;
            this.Director = string.Empty;
            this.Genre = string.Empty;
        }

        /// <summary>
        /// Primary key.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Movie title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Movie director.
        /// </summary>
        public string Director { get; set; }

        /// <summary>
        /// Year the movie was released.
        /// </summary>
        public int ReleaseYear { get; set; }

        /// <summary>
        /// Movie genre.
        /// </summary>
        public string Genre { get; set; }
    }
}