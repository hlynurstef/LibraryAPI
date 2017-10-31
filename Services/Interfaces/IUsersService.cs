using System;
using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;

namespace LibraryAPI.Services {
    public interface IUsersService {
        IEnumerable<UserDTOLite> GetUsers ();
        IEnumerable<UserDTOLite> GetUsersQuery (DateTime queryDate);
        IEnumerable<UserDTOLite> GetUsersOnDuration (DateTime queryDate, int loanDuration);
        UserDTO AddUser (UserView user);
        UserDTO GetUserById (int userId);
        void DeleteUser (int userId);
        void UpdateUser (UserView user, int userId);

    }
}