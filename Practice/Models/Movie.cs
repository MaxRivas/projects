using System;
using System.ComponentModel.DataAnnotations;
using Practice.Interfaces;

namespace Practice.Models
{
    public class Movie : IMovie
    {
        public Movie()
        {            
        }

        /// <summary>
        /// Gets identifier of Movie
        /// </summary>
        /// <value>
        /// Integer ID value
        /// </value>
        public int Id { get; set;}

        /// <summary>
        /// Gets the price of Movie
        /// </summary>
        /// <value>
        /// Decimal price
        /// </value>
        public decimal Price { get; set;}

        /// <summary>
        /// Gets title of Movie
        /// </summary>
        /// <value>
        /// String value
        /// </value>
        public string Title { get; set;}

        /// <summary>
        /// Gets Genre of Movie
        /// </summary>
        /// <value>
        /// String value
        /// </value>
        public string Genre { get; set;} 

        /// <summary>
        /// Gets release date of Movie
        /// </summary>
        /// <value>
        /// Date and time of release
        /// </value>
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set;} 
    }
}
