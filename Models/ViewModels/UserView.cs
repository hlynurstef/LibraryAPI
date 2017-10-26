namespace LibraryAPI.Models.ViewModels
{
    /// <summary>
    /// A view model of a user.
    /// This is the model that is used when a user is creating and updating a user.
    /// </summary>
    public class UserView
    {
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