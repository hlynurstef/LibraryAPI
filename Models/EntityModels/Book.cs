using System;

namespace LibraryAPI.Models.EntityModels
{
    /// <summary>
    /// An entity model of a book that is used as the 
    /// data structure in our database.
    /// </summary>
    public class Book
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
        /// Whether the book is available for loan.
        /// </summary>
        public bool Available { get; set; }
        
    }
}