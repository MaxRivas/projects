using System;

namespace Practice.Interfaces
{
    /// <summary>
    /// Interface representing a movie
    /// </summary>
    public interface IMovie
    {
        /// <summary>
        /// Gets identifier of Movie
        /// </summary>
        /// <value>
        /// Integer ID value
        /// </value>
        int Id { get; }

        /// <summary>
        /// Gets the price of Movie
        /// </summary>
        /// <value>
        /// Decimal price
        /// </value>
        decimal Price { get; }

        /// <summary>
        /// Gets title of Movie
        /// </summary>
        /// <value>
        /// String value
        /// </value>
        string Title { get; }

        /// <summary>
        /// Gets Genre of Movie
        /// </summary>
        /// <value>
        /// String value
        /// </value>
        string Genre { get;}

        /// <summary>
        /// Gets release date of Movie
        /// </summary>
        /// <value>
        /// Date and time of release
        /// </value>
        DateTime ReleaseDate { get; } 
    }
}
