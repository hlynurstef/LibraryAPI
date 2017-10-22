using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;

namespace LibraryAPI.Repositories
{
    public interface IUsersRepository
    {
        IEnumerable<UserDTOLite> GetUsers();
        UserDTO AddUser(UserView user);
        UserDTO GetUserById(int userId);
        void DeleteUser(int userId);
    }
}