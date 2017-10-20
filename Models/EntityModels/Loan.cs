namespace LibraryAPI.EntityModels
{
    /// <summary>
    /// An entity model of a loan that is used as the
    /// data structure in our database.
    /// </summary>
    public class Loan
    {
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
    }
}