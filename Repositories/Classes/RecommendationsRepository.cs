using System.Collections.Generic;
using System.Linq;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Repositories;
using LibraryAPI.Exceptions;

namespace LibraryAPI.Repositories
{
    public class RecommendationsRepository : IRecommendationsRepository
    {
        private readonly AppDataContext _db;

        public RecommendationsRepository(AppDataContext db) {
            _db = db;
        }

        public IEnumerable<BookDTO> GetRecommendationsForUser(int userId)
        {
            // Get the user
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

            // Should not include books that the user has already taken out on a loan

            if (user.LoanHistory != null) {
                foreach (var book in user.LoanHistory)
                {
                                        
                }
            }

            List<BookDTO> recommendations = new List<BookDTO>();
            
            return recommendations;
        }
    }
}