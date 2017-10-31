using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Repositories;
using System;

namespace LibraryAPI.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _repo;

        public UsersService(IUsersRepository repo) {
            _repo = repo;
        }

        public UserDTO AddUser(UserView user)
        {
            return _repo.AddUser(user);
        }

        public void DeleteUser(int userId)
        {
            _repo.DeleteUser(userId);
        }

        public void UpdateUser(UserView user, int userId) 
        {
            _repo.UpdateUser(user, userId);
        }

        public UserDTO GetUserById(int userId)
        {
            return _repo.GetUserById(userId);
        }

        public IEnumerable<UserDTOLite> GetUsers() {
            var users = _repo.GetUsers();
            return users;
        }

        public IEnumerable<UserDTOLite> GetUsersOnLoan(DateTime queryDate){
            return _repo.GetUsersOnLoan(queryDate);
        }
    }
}