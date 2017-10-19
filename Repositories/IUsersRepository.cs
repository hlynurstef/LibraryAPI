using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;

namespace LibraryAPI.Repositories
{
    public interface IUsersRepository
    {
         IEnumerable<UserDTO> GetUsers();
    }
}