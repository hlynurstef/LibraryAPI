using System;
using System.Linq;
using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Models.EntityModels;
using Microsoft.EntityFrameworkCore;
using LibraryAPI.Exceptions;

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
                PhoneNumber = user.PhoneNumber,
                Deleted = false
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
                PhoneNumber = userEntity.PhoneNumber,
                LoanHistory = new List<LoanDTO>()
            };
        }

        public void DeleteUser(int userId)
        {
            var user = (from u in _db.Users
                        where u.Id == userId
                        select u).SingleOrDefault();
            
            if (user == null) {
                throw new NotFoundException("User with id " + userId + " does not exist");
            }

            user.Deleted = true;

            try {
                _db.SaveChanges();
            }
            catch(DbUpdateException e) {
                Console.WriteLine(e);
            }
        }

        public void UpdateUser(UserView updatedUser, int userId)
        {
            var user = (from u in _db.Users
                        where u.Id == userId
                        && u.Deleted == false
                        select u).SingleOrDefault();
            
            if (user == null) {
                throw new NotFoundException("User with id " + userId + " does not exist");
            }

            user.Name = updatedUser.Name;
            user.Address = updatedUser.Address;
            user.Email = updatedUser.Email;
            user.PhoneNumber = updatedUser.PhoneNumber;
            
            try {
                _db.SaveChanges();
            }
            catch(DbUpdateException e) {
                Console.WriteLine(e);
            }
        }

        public UserDTO GetUserById(int userId)
        {
            var user = (from u in _db.Users
                        where u.Id == userId
                        && u.Deleted == false
                        select new UserDTO {
                            Id = u.Id,
                            Name = u.Name,
                            Address = u.Address,
                            Email = u.Email,
                            PhoneNumber = u.PhoneNumber,
                            LoanHistory = (from l in _db.Loans
                                            where l.UserId == userId
                                            join b in _db.Books on l.BookId equals b.Id
                                            select new LoanDTO {
                                                Id = l.Id,
                                                BookTitle = b.Title,
                                                LoanDate = l.LoanDate
                                            }).ToList()
                        }).SingleOrDefault();
            if (user == null) {
                throw new NotFoundException("User with id " + userId + " does not exist");
            }

            return user;
        }

        public IEnumerable<UserDTOLite> GetUsers() {
            var users = (from u in _db.Users
                        where u.Deleted == false
                        select new UserDTOLite {
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
