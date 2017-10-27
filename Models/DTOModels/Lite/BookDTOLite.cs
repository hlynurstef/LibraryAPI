using System;
using System.Collections.Generic;
using LibraryAPI.Models.EntityModels;

namespace LibraryAPI.Models.DTOModels
{
    /// <summary>
    /// A DTO model of a book used when displaying
    /// information to the user.
    /// </summary>
    public class BookDTOLite
    {
        /// <summary>
        /// The Id of the book.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The title of the book.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// The Author of the book.
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// The date of release.
        /// </summary>
        public DateTime ReleaseDate { get; set; }
        /// <summary>
        /// The ISBN number of the book.
        /// </summary>
        public string ISBN { get; set; }
        /// <summary>
        /// Book Availability
        /// </summary>
        public bool Available { get; set; }
        /// <summary>
        /// List of all reviews for this book.
        /// </summary>
        public List<Review> Reviews { get; set; }
    }
}