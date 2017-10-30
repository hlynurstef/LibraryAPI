using System;

namespace LibraryAPI.Models.DTOModels
{
    /// <summary>
    /// A DTO model of a loan used when displaying 
    /// information to the user.
    /// </summary>
    public class LoanDTO
    {
        /// <summary>
        /// The Id of the loan.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The title of the book on loan.
        /// </summary>
        public string BookTitle { get; set; }
        /// <summary>
        /// The date of the loan.
        /// </summary>
        public DateTime LoanDate { get; set; }
        
        /// <summary>
        /// The return date of the book
        /// </summary>
        /// <returns></returns>
        public DateTime? EndDate { get; set; }
    }
}