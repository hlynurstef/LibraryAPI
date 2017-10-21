using System;
using System.Linq;
using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Models.EntityModels;

namespace LibraryAPI.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AppDataContext _db;

        public UsersRepository(AppDataContext db) {
            _db = db;
        }

        public UserDTO AddUser(UserView user) {
            var userEntity = new User {
                Name = user.Name,
                Address = user.Address,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            _db.Users.Add(userEntity);

            try{
                _db.SaveChanges();
            }
            catch(System.Exception e){
                Console.WriteLine(e);
            }

            return new UserDTO {
                Id = userEntity.Id,
                Name = userEntity.Name,
                Address = userEntity.Address,
                Email = userEntity.Email,
                PhoneNumber = userEntity.PhoneNumber
            };
        }

        public UserDTO GetUserById(int userID)
        {
            var user = (from u in _db.Users
                        where u.Id == userID
                        select new UserDTO {
                            Id = u.Id,
                            Name = u.Name,
                            Address = u.Address,
                            Email = u.Email,
                            PhoneNumber = u.PhoneNumber
                        }).SingleOrDefault();

            return user;
        }

        public IEnumerable<UserDTO> GetUsers() {
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
