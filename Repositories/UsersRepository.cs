using System;
using System.Linq;
using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;

namespace LibraryAPI.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AppDataContext _db;

        public UsersRepository(AppDataContext db) {
            _db = db;
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            var users = (from u in _db.Users
                        select new UserDTO {
                            Id = u.Id,
                            Name = u.Name,
                            Address = u.Address,
                            Email = u.Email,
                            PhoneNumber = u.PhoneNumber
                        }).ToList();
            return users;
        }
    }
}
