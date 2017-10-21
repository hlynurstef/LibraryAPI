using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;

namespace LibraryAPI.Services
{
    public interface IUsersService
    {
         IEnumerable<UserDTO> GetUsers();
         UserDTO AddUser(UserView user);
         UserDTO GetUserById(int userId);
    }
}