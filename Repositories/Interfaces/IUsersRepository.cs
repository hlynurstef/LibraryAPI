using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;
using System;

namespace LibraryAPI.Repositories
{
    public interface IUsersRepository
    {
        IEnumerable<UserDTOLite> GetUsers();
        UserDTO AddUser(UserView user);
        UserDTO GetUserById(int userId);
        void DeleteUser(int userId);
        void UpdateUser(UserView user, int userId);
        IEnumerable<UserDTOLite> GetUsersQuery(DateTime queryDate);
        IEnumerable<UserDTOLite> GetUsersOnDuration(DateTime queryDate,int loanDuration);
    }
}