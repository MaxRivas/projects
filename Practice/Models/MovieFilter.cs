using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Practice.Interfaces;

namespace Practice.Models
{
    public class MovieFilter : IMovieFilter
    {
        // <summary>
        /// Gets or sets the list of movies
        /// </summary>
        /// <value></value>
        public IList<IMovie> Movies { get; set; }

        /// <summary>
        /// Gets or sets  the list of genres
        /// </summary>
        /// <value></value>
        public SelectList Genres { get; set; }

        /// <summary>
        /// Gets or sets the selected genre
        /// </summary>
        /// <value></value>
        public string GenreOfMovie { get; set; }

        /// <summary>
        /// Gets or sets partial query string
        /// </summary>
        /// <value></value>
        public string SearchString { get; set; }
    }
}
