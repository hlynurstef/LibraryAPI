namespace LibraryAPI.Models.EntityModels
{
    /// <summary>
    /// An entity model of a book that is used as the
    /// data structure in our database.
    /// This model should not be exposed to the user!
    /// </summary>
    public class Review
    {
        
        /// <summary>
        /// The User Id of the review.
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// The Book Id of the review.
        /// </summary>
        public int BookId { get; set; }
        /// <summary>
        /// The review text.
        /// </summary>
        public string ReviewText { get; set; }
        /// <summary>
        /// The star count of the review.
        /// </summary>
        public int Stars { get; set; }
    }
}