using System.Collections.Generic;

namespace LibraryAPI.Models.DTOModels
{
    /// <summary>
    /// A DTO model of a book review used when displaying
    /// information to the user.
    /// </summary>
    public class BookReviewsDTO
    {
        /// <summary>
        /// The Id of the Book.
        /// </summary>
        public int BookId { get; set; }
        /// <summary>
        /// The Title of the Book.
        /// </summary>
        public string BookTitle { get; set; }
        /// <summary>
        /// The Author of the Book
        /// </summary>
        public string BookAuthor { get; set; }
        /// <summary>
        /// The ISBN of the book
        /// </summary>
        public string ISBN { get; set; }
        /// <summary>
        /// A list of stars.
        /// </summary>
        public IEnumerable<int> Stars { get; set; }
    }
}