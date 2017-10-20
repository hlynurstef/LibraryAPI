namespace LibraryAPI.Models.DTOModels
{
    /// <summary>
    /// A DTO model of a user used when sending information within the application.
    /// This model should not be exposed to the user!
    /// </summary>
    public class UserDTO
    {
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
    }
}