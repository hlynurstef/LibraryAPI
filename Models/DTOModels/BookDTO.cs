using System;

namespace LibraryAPI.Models.DTOModels
{
    /// <summary>
    /// A DTO model of a book used when sending information within the application.
    /// This model should not be exposed to the user!
    /// </summary>
    public class BookDTO
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
    }
}