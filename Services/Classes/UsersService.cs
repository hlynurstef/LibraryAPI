using System;
using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Repositories;

namespace LibraryAPI.Services {
    public class UsersService : IUsersService {
        private readonly IUsersRepository _repo;

        public UsersService (IUsersRepository repo) {
            _repo = repo;
        }

        public UserDTO AddUser (UserView user) {
            return _repo.AddUser (user);
        }

        public void DeleteUser (int userId) {
            _repo.DeleteUser (userId);
        }

        public void UpdateUser (UserView user, int userId) {
            _repo.UpdateUser (user, userId);
        }

        public UserDTO GetUserById (int userId) {
            return _repo.GetUserById (userId);
        }

        public IEnumerable<UserDTOLite> GetUsers () {
            var users = _repo.GetUsers ();
            return users;
        }

        public IEnumerable<UserDTOLite> GetUsersQuery (DateTime queryDate) {
            return _repo.GetUsersQuery (queryDate);
        }

        public IEnumerable<UserDTOLite> GetUsersOnDuration (DateTime queryDate, int loanDuration) {
            return _repo.GetUsersOnDuration (queryDate, loanDuration);
        }
    }
}