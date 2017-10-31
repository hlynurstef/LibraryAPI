using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models.ViewModels {
    /// <summary>
    /// A view model of a loan.
    /// This is the model that is used when a user is updating an existing loan.
    /// </summary>
    public class LoanView {
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