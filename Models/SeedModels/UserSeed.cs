using System.Collections.Generic;

namespace LibraryAPI.Models.SeedModels {
    /// <summary>
    /// An entity model of a user that is used as the 
    /// data structure in our database.
    /// This model should not be exposed to the user!
    /// </summary>
    public class UserSeed {
        /// <summary>
        /// The Id of the user.
        /// </summary>
        public int vinur_id { get; set; }
        /// <summary>
        /// The name of the user.
        /// </summary>
        public string fornafn { get; set; }
        /// <summary>
        /// The Home address of the user.
        /// </summary>
        public string eftirnafn { get; set; }
        /// <summary>
        /// The email of the user.
        /// </summary>
        public string netfang { get; set; }
        /// <summary>
        /// The phone number of the user.
        /// </summary>
        public string heimilisfang { get; set; }
        /// <summary>
        /// If true, the user has been deleted and should not show up in any get request
        /// </summary>
        public List<LoanSeed> lanasafn { get; set; }
    }
}