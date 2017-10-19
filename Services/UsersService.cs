using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Repositories;

namespace LibraryAPI.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _repo;

        public UsersService(IUsersRepository repo) {
            _repo = repo;
        }
        public IEnumerable<UserDTO> GetUsers() {
            var users = _repo.GetUsers();
            return users;
        }
    }
}