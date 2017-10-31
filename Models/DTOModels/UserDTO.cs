using System.Collections.Generic;

namespace LibraryAPI.Models.DTOModels {
    /// <summary>
    /// A DTO model of a user used when displaying
    /// information to the user.
    /// </summary>
    public class UserDTO {
        /// <summary>
        /// The Id of the user.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The name of the user.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The Home address of the user.
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// The email of the user.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// The phone number of the user.
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// The history of loans for this user.
        /// </summary>
        public IEnumerable<LoanDTO> LoanHistory { get; set; }

    }
}