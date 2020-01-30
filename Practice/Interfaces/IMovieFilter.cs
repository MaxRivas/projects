using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Practice.Interfaces
{
    /// <summary>
    /// Interface to define a filter
    /// </summary>
    interface IMovieFilter
    {
        /// <summary>
        /// Gets the list of movies
        /// </summary>
        /// <value></value>
        IList<IMovie> Movies { get;}

        /// <summary>
        /// Gets  the list of genres
        /// </summary>
        /// <value></value>
        SelectList Genres { get; }

        /// <summary>
        /// Gets the selected genre
        /// </summary>
        /// <value></value>
        string GenreOfMovie { get; }

        /// <summary>
        /// Gets partial query string
        /// </summary>
        /// <value></value>
        string SearchString { get; }
    }
}