using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;
using System;

namespace LibraryAPI.Services
{
    public interface IUsersService
    {
         IEnumerable<UserDTOLite> GetUsers();
         IEnumerable<UserDTOLite> GetUsersOnLoan(DateTime queryDate);
         UserDTO AddUser(UserView user);
         UserDTO GetUserById(int userId);
         void DeleteUser(int userId);
         void UpdateUser(UserView user, int userId);
    }
}