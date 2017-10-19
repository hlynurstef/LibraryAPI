using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;

namespace LibraryAPI.Services
{
    public interface IUsersService
    {
         IEnumerable<UserDTO> GetUsers();
    }
}