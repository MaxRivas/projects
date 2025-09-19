namespace MoviesApi.Models
{
    public class Movie
    {
        public Movie()
        {
            this.Title = string.Empty;
            this.Director = string.Empty;
            this.Genre = string.Empty;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public int ReleaseYear { get; set; }
        public string Genre { get; set; }
    }
}