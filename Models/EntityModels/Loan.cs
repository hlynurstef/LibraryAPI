using System;

namespace LibraryAPI.Models.EntityModels {
    /// <summary>
    /// An entity model of a loan that is used as the
    /// data structure in our database.
    /// This model should not be exposed to the user!
    /// </summary>
    public class Loan {
        /// <summary>
        /// The Id of the loan.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The User Id of this loan.
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// The Book Id of this loan.
        /// </summary>
        public int BookId { get; set; }
        /// <summary>
        /// The date of the loan.
        /// </summary>

        public DateTime LoanDate { get; set; }
        /// <summary>
        /// Whether this book has been returned or not.
        /// </summary>
        public bool HasBeenReturned { get; set; }

        /// <summary>
        /// The return date of the book
        /// </summary>
        /// <returns></returns>
        public DateTime? EndDate { get; set; }
    }
}