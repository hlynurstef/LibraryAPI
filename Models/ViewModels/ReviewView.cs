using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models.ViewModels
{
    /// <summary>
    /// A viewmodel of a review
    /// This model is used when creating and updating a review for a book
    /// </summary>
    public class ReviewView
    {
        /// <summary>
        /// The review itself.
        /// </summary>
        public string ReviewText { get; set; }

        /// <summary>
        /// Stars for the review, range between 1 & 5
        /// </summary>
        [Required]
        [RegularExpression("^[1-5]{1}", ErrorMessage = "Stars must be on the scale 1 to 5")]
        // int? is used because it needs to bo nullable in order
        // for Required to work.
        public int? Stars { get; set; }
    }
}