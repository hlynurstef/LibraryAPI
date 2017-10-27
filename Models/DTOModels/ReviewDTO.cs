namespace LibraryAPI.Models.DTOModels
{
    /// <summary>
    /// A DTO model of a review used when displaying
    /// information to the user
    /// </summary>
    public class ReviewDTO
    {
        /// <summary>
        /// The Id of the user that gave this review.
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// The Title of the book reviewed.
        /// </summary>
        public string BookTitle { get; set; }
        /// <summary>
        /// The Id of the book reviewed.
        /// </summary>
        public int BookId { get; set; }
        /// <summary>
        /// The text of the review.
        /// </summary>
        public string ReviewText { get; set; }
        /// <summary>
        /// The star count of this review.
        /// </summary>
        public int Stars { get; set; }
    }
}